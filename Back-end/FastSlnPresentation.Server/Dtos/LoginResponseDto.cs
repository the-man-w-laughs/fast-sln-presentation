using FastSlnPresentation.BLL.DTOs;

namespace FastSlnPresentation.Server.Dtos
{
    public class LoginResponseDto
    {
        public UserResponseDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public LoginResponseDto(string accessToken, string refreshToken, UserResponseDto user)
        {
            this.User = user;
            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
        }
    }
}
