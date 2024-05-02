namespace FastSlnPresentation.BLL.DTOs
{
    // Класс для представления подписки
    public class SubscriptionRequestDto
    {
        public int UserId { get; set; } // Идентификатор пользователя (внешний ключ, ссылается на UserId в Users)
        public int PlanId { get; set; } // Идентификатор плана (внешний ключ, ссылается на PlanId в Plans)
        public DateTime StartDate { get; set; } // Дата начала подписки
    }
}
