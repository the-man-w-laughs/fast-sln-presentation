using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
using FastSlnPresentation.DAL.Models;
using FastSlnPresentation.Server.Dtos;
using FastSlnPresentation.Server.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FastSlnPresentation.Server.Controllers
{
    [Route("/")]
    public class AuthenticationController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;
        private readonly SubscriptionService _subscriptionService;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthenticationController(
            ILogger<UsersController> logger,
            UserService userService,
            SubscriptionService subscriptionService,
            RefreshTokenService refreshTokenService
        )
        {
            _logger = logger;
            _userService = userService;
            _subscriptionService = subscriptionService;
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Войти в систему.
        /// </summary>
        /// <param name="userLoginDto">Данные для входа пользователя (email и пароль).</param>
        /// <returns>
        /// HTTP ответ, содержащий JWT-токен и информацию о пользователе,
        /// если вход выполнен успешно; 401 Unauthorized, если учетные данные неверны.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = await _userService.CheckPassword(userLoginDto.Email, userLoginDto.Password);

            if (user == null)
            {
                return Unauthorized("Неправильный email или пароль.");
            }

            await _refreshTokenService.RevokeUserTokensAsync(user.Id);

            var role = GetUserRole(user.RoleId);

            if (role == Roles.User)
            {
                await _subscriptionService.GetActiveSubscription(user.Id);
            }

            var (accessToken, refreshToken) = await GenerateTokens(user);
            var loginResponse = new LoginResponseDto(accessToken, refreshToken.Token, user);

            return Ok(loginResponse);
        }

        /// <summary>
        /// Обновить токен доступа.
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        {
            var principal = GetPrincipalFromExpiredToken(requestDto.AccessToken);
            var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(
                principal.GetUserId(),
                requestDto.RefreshToken
            );

            if (refreshToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            await _refreshTokenService.RevokeTokenAsync(refreshToken.Id);

            if (
                refreshToken.CreatedAt.AddSeconds(AuthOptions.RefreshTokenExpiresTime)
                < DateTime.UtcNow.ToLocalTime()
            )
            {
                return Unauthorized("Expired refresh token.");
            }

            var role = GetUserRole(refreshToken.UserId);

            if (role == Roles.User)
            {
                await _subscriptionService.GetActiveSubscription(refreshToken.UserId);
            }

            var user = await _userService.GetUserById(refreshToken.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var (newAccessToken, newRefreshToken) = await GenerateTokens(user);

            return Ok(new RefreshTokenResponseDto(newAccessToken, newRefreshToken.Token));
        }

        private string GetUserRole(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return Roles.Admin;
                case 2:
                    return Roles.User;
                default:
                    throw new Exception("Internal server error.");
            }
        }

        private async Task<(string AccessToken, RefreshToken RefreshToken)> GenerateTokens(
            UserResponseDto user
        )
        {
            var claims = CreateClaims(user);
            var jwt = CreateJwtToken(claims);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refreshToken = await _refreshTokenService.GenerateTokenAsync(user.Id);
            return (accessToken, refreshToken);
        }

        private List<Claim> CreateClaims(UserResponseDto user)
        {
            var role = GetUserRole(user.RoleId);
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };
        }

        private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
        {
            var signingKey = AuthOptions.GetSymmetricSecurityKey();
            var signingCredentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256
            );

            return new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(AuthOptions.TokenExpiresTime),
                signingCredentials: signingCredentials
            );
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = AuthOptions.GetSymmetricSecurityKey();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out SecurityToken securityToken
            );
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (
                jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
