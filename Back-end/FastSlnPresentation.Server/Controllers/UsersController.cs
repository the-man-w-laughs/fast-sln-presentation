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

        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        /// <returns>Список всех пользователей.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();

            return Ok(result);
        }

        /// <summary>
        /// Получить пользователя по jwt.
        /// </summary>
        /// <returns>Данные пользователя.</returns>
        [Authorize]
        [HttpGet("token")]
        public async Task<IActionResult> GetUserByJwt()
        {
            var id = User.GetUserId();
            var result = await _userService.GetUserById(id);

            return Ok(result);
        }

        /// <summary>
        /// Получить пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Данные пользователя.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);

            return Ok(result);
        }

        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="userRequestDto">DTO запроса, содержащая данные о новом пользователе.</param>
        /// <returns>Созданный пользователь.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> CreateUser(UserRequestDto userRequestDto)
        {
            var result = await _userService.CreateUser(userRequestDto);

            return Ok(result);
        }

        /// <summary>
        /// Удалить пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Удаленный пользователь.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);

            return Ok(result);
        }
    }
}
