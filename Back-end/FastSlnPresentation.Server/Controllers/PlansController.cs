using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
using FastSlnPresentation.Server.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Route("[controller]")]
    public class PlansController : Controller
    {
        private readonly ILogger<PlansController> _logger;
        private readonly PlanService _planService;

        public PlansController(ILogger<PlansController> logger, PlanService userService)
        {
            _logger = logger;
            _planService = userService;
        }

        /// <summary>
        /// Получить все планы.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _planService.GetAllPlans();
            return Ok(plans);
        }

        /// <summary>
        /// Создать новый план.
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePlan(PlanRequestDto planRequestDto)
        {
            var createdPlan = await _planService.CreatePlan(planRequestDto);
            return Ok(createdPlan);
        }

        /// <summary>
        /// Удалить план, если у него нет активных или будущих подписок.
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlanIfNoActiveOrFutureSubscriptions(int id)
        {
            var deletedPlan = await _planService.DeletePlanIfNoActiveOrFutureSubscriptions(id);

            return Ok(deletedPlan);
        }
    }
}
