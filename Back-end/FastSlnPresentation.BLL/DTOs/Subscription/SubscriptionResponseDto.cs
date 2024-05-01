namespace FastSlnPresentation.BLL.DTOs
{
    // Класс для представления подписки
    public class SubscriptionResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Идентификатор пользователя (внешний ключ, ссылается на UserId в Users)
        public int PlanId { get; set; } // Идентификатор плана (внешний ключ, ссылается на PlanId в Plans)
        public DateTime StartDate { get; set; } // Дата начала подписки
        public DateTime EndDate { get; set; } // Дата окончания подписки
        public bool IsActive { get; set; } // Активна ли подписка (логическое значение, по умолчанию true)
    }
}
