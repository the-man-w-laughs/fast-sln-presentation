using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
using FastSlnPresentation.Server.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public AuthenticationController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

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
