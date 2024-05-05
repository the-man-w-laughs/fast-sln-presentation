namespace FastSlnPresentation.DAL.Models
{
    // Класс для представления плана
    public class RefreshToken
    {
        public int Id { get; set; } // Уникальный идентификатор плана (первичный ключ)
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Связь с подписками
        public virtual User User { get; set; }
    }
}
