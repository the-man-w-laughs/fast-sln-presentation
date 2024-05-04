namespace FastSlnPresentation.BLL.DTOs
{
    // Класс для представления плана
    public class PlanResponseDto
    {
        public int Id { get; set; } // Уникальный идентификатор плана (первичный ключ)
        public string Name { get; set; } // Название плана
        public decimal Price { get; set; } // Стоимость плана
        public int Duration { get; set; } // Длительность плана в днях
        public string Description { get; set; } // Описание плана
    }
}
