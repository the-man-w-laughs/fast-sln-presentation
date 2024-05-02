using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
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

        // Получение всех планов
        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _planService.GetAllPlans();
            return Ok(plans);
        }

        // Создание нового плана
        [HttpPost]
        public async Task<IActionResult> CreatePlan(PlanRequestDto planRequestDto)
        {
            var createdPlan = await _planService.CreatePlan(planRequestDto);
            return Ok(createdPlan);
        }

        // Удаление плана, если у него нет активных или будущих подписок
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlanIfNoActiveOrFutureSubscriptions(int id)
        {
            var deletedPlan = await _planService.DeletePlanIfNoActiveOrFutureSubscriptions(id);

            return Ok(deletedPlan);
        }
    }
}
