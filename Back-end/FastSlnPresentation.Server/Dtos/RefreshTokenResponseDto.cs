using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastSlnPresentation.Server.Dtos
{
    public class RefreshTokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public RefreshTokenResponseDto(string accessToken, string refreshToken)
        {
            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
        }
    }
}
