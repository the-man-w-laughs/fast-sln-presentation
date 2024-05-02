using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.DBServices;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Route("[controller]")]
    public class SubscriptionsController : Controller
    {
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionsController(
            ILogger<SubscriptionsController> logger,
            SubscriptionService subscriptionService
        )
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        // Создание новой подписки
        [HttpPost]
        public async Task<IActionResult> CreateSubscription(
            SubscriptionRequestDto subscriptionRequestDto
        )
        {
            var createdSubscription = await _subscriptionService.CreateSubscription(
                subscriptionRequestDto
            );
            return Ok(createdSubscription);
        }

        // Получение подписок пользователя
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserSubscriptions(int userId)
        {
            var subscriptions = await _subscriptionService.GetUserSubscriptions(userId);

            return Ok(subscriptions);
        }

        // Получение активной подписки пользователя
        [HttpGet("active/{userId:int}")]
        public async Task<IActionResult> GetActiveSubscription(int userId)
        {
            var activeSubscription = await _subscriptionService.GetActiveSubscription(userId);

            if (activeSubscription == null)
            {
                return NotFound($"Активная подписка для пользователя с id {userId} не найдена.");
            }

            return Ok(activeSubscription);
        }

        // Удаление подписки по идентификатору
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var deletedSubscription = await _subscriptionService.DeleteSubscription(id);

            return Ok(deletedSubscription);
        }
    }
}
