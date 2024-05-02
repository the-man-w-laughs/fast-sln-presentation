using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
using FastSlnPresentation.Server.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;

        public UsersController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();

            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserById(id);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> CreateUser(UserRequestDto userRequestDto)
        {
            var result = await _userService.CreateUser(userRequestDto);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);

            return Ok(result);
        }
    }
}
