namespace FastSlnPresentation.BLL.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; } // Уникальный идентификатор пользователя (первичный ключ)
        public string Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Электронный адрес пользователя (уникальное значение)
        public DateTime CreatedAt { get; set; } // Дата и время создания пользователя (по умолчанию текущая дата и время)
        public int RoleId { get; set; } // Идентификатор роли пользователя (внешний ключ, ссылается на RoleId в Roles)
    }
}
