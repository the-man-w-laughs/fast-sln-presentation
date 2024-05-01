namespace FastSlnPresentation.BLL.DTOs
{
    public class UserRequestDto
    {
        public string Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Электронный адрес пользователя (уникальное значение)
        public int RoleId { get; set; } // Идентификатор роли пользователя (внешний ключ, ссылается на RoleId в Roles)
        public string Password { get; set; }
    }
}
