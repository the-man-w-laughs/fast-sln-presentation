using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.DAL.Models;

namespace FastSlnPresentation.BLL.AutomapperProfiles
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            CreateMap<UserLoginDto, User>();
            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserResponseDto>();
        }
    }
}
