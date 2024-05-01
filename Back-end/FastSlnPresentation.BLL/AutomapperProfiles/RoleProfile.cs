using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.DAL.Models;

namespace FastSlnPresentation.BLL.AutomapperProfiles
{
    public class RoleProfile : BaseProfile
    {
        public RoleProfile()
        {
            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();
        }
    }
}
