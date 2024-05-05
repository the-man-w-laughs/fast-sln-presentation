using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastSlnPresentation.Server.Dtos
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
