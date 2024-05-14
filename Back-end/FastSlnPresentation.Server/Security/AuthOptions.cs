using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FastSlnPresentation.Server.Security
{
    public class AuthOptions
    {
        public const int RefreshTokenExpiresTime = 30 * 24 * 60 * 60;
        public const int TokenExpiresTime = 2 * 60 * 60;
        public const string AuthenticationScheme = "Bearer";
        public const string ISSUER = "fast-sln-presentation-server"; // издатель токена
        public const string AUDIENCE = "fast-sln-presentation-client"; // потребитель токена
        const string KEY = "fast-sln-presentation-key1234567"; // ключ для шифрации

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
