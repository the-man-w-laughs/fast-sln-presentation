namespace FastSlnPresentation.DAL.Models
{
    // Класс для представления подписки
    public class Subscription
    {
        public int Id { get; set; } // Уникальный идентификатор подписки (первичный ключ)
        public int UserId { get; set; } // Идентификатор пользователя (внешний ключ, ссылается на UserId в Users)
        public int PlanId { get; set; } // Идентификатор плана (внешний ключ, ссылается на PlanId в Plans)
        public DateTime StartDate { get; set; } // Дата начала подписки
        public DateTime EndDate { get; set; } // Дата окончания подписки
        public DateTime CreatedAt { get; set; } // Дата и время создания подписки (по умолчанию текущая дата и время)

        // Связь с пользователем
        public virtual User User { get; set; }

        // Связь с планом
        public virtual Plan Plan { get; set; }
    }
}
