using Microsoft.IdentityModel.Tokens;

namespace FastSlnPresentation.Server.Security
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Расширяет IServiceCollection для добавления аутентификации с использованием Bearer и настройки JWT Bearer.
        /// </summary>
        /// <param name="services">Коллекция сервисов для настройки аутентификации.</param>
        /// <returns>Возвращает коллекцию сервисов после добавления аутентификации.</returns>
        public static IServiceCollection AddJwtBearerAuthentication(
            this IServiceCollection services
        )
        {
            // Добавляем аутентификацию с использованием Bearer
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    // Настройка параметров валидации токена
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Валидация издателя токена
                        ValidateIssuer = true,
                        // Установка ожидаемого издателя
                        ValidIssuer = AuthOptions.ISSUER,
                        // Валидация потребителя токена
                        ValidateAudience = true,
                        // Установка ожидаемой аудитории
                        ValidAudience = AuthOptions.AUDIENCE,
                        // Валидация времени существования токена
                        ValidateLifetime = true,
                        // Установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // Валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            return services;
        }
    }
}
