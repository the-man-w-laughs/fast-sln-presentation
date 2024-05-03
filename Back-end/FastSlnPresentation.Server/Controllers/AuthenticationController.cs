using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
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
        private const int TokenExpiresTime = 120;
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;
        private readonly SubscriptionService _subscriptionService;

        public AuthenticationController(
            ILogger<UsersController> logger,
            UserService userService,
            SubscriptionService subscriptionService
        )
        {
            _logger = logger;
            _userService = userService;
            _subscriptionService = subscriptionService;
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
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            // Check if the user exists and the password is correct
            var user = await _userService.CheckPassword(userLoginDto.Email, userLoginDto.Password);

            if (user == null)
            {
                // Return 401 Unauthorized if the user is not found or password is incorrect
                return Unauthorized("Invalid email or password.");
            }

            // Determine the user's role based on their RoleId
            string role;
            switch (user.RoleId)
            {
                case 1:
                    role = Roles.Admin;
                    break;
                case 2:
                    role = Roles.User;
                    break;
                default:
                    // Return 400 Bad Request if the role is invalid
                    return BadRequest("Invalid role.");
            }

            switch (role)
            {
                case Roles.Admin:
                    break;
                case Roles.User:
                    await _subscriptionService.GetActiveSubscription(user.Id);
                    break;
            }

            // Create claims for the user's email, role, and other relevant user information
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Example: Add user ID as a claim
                new Claim(ClaimTypes.Name, user.Email), // Example: Add user email as a claim
                new Claim(ClaimTypes.Role, role)
            };

            // Create a JWT token with the specified issuer, audience, claims, and expiration time
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(TokenExpiresTime),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256
                )
            );

            // Return the JWT token
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new { access_token = token, user });
        }
    }
}
