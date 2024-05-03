using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Exceptions;
using FastSlnPresentation.BLL.Services.DBServices;
using FastSlnPresentation.Server.Security;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Создать подписку.
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("")]
        public async Task<IActionResult> CreateSubscription(
            SubscriptionRequestDto subscriptionRequestDto
        )
        {
            var activeSubscription = await _subscriptionService.CreateSubscription(
                subscriptionRequestDto
            );

            return Ok(activeSubscription);
        }

        /// <summary>
        /// Получить все подписки пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Список подписок пользователя.</returns>
        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserSubscriptions(int userId)
        {
            var subscriptions = await _subscriptionService.GetUserSubscriptions(userId);

            return Ok(subscriptions);
        }

        /// <summary>
        /// Получить активную подписку пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Активная подписка пользователя или сообщение об отсутствии.</returns>
        [Authorize]
        [HttpGet("active/{userId:int}")]
        public async Task<IActionResult> GetActiveSubscription(int userId)
        {
            var activeSubscription = await _subscriptionService.GetActiveSubscription(userId);

            return Ok(activeSubscription);
        }

        /// <summary>
        /// Удалить подписку по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор подписки.</param>
        /// <returns>Удаленная подписка.</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var deletedSubscription = await _subscriptionService.DeleteSubscription(id);

            return Ok(deletedSubscription);
        }
    }
}
