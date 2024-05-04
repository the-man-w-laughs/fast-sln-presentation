namespace FastSlnPresentation.DAL.Models
{
    // Класс для представления плана
    public class Plan
    {
        public int Id { get; set; } // Уникальный идентификатор плана (первичный ключ)
        public string Name { get; set; } // Название плана
        public decimal Price { get; set; } // Стоимость плана
        public int Duration { get; set; } // Длительность плана в днях
        public string Description { get; set; } // Описание плана

        // Связь с подписками
        public virtual ICollection<Subscription> Subscriptions { get; set; } =
            new List<Subscription>();
    }
}
