namespace FastSlnPresentation.DAL.Models
{
    // Класс для представления роли
    public class Role
    {
        public int Id { get; set; } // Уникальный идентификатор роли (первичный ключ)
        public string Name { get; set; } // Название роли

        // Связь с пользователями
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
