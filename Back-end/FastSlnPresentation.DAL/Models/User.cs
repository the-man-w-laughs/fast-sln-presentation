namespace FastSlnPresentation.DAL.Models
{
    public class User
    {
        public int Id { get; set; } // Уникальный идентификатор пользователя (первичный ключ)
        public string Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Электронный адрес пользователя (уникальное значение)
        public string PasswordHash { get; set; } // Хэш пароля
        public string Salt { get; set; } // Соль
        public DateTime CreatedAt { get; set; } // Дата и время создания пользователя (по умолчанию текущая дата и время)
        public int RoleId { get; set; } // Идентификатор роли пользователя (внешний ключ, ссылается на RoleId в Roles)

        // Связь с ролью
        public virtual Role Role { get; set; }

        // Связь с подписками
        public virtual ICollection<Subscription> Subscriptions { get; set; } =
            new List<Subscription>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } =
            new List<RefreshToken>();
    }
}
