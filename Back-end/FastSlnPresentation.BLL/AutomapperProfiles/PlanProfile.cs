using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.DAL.Models;

namespace FastSlnPresentation.BLL.AutomapperProfiles
{
    public class PlanProfile : BaseProfile
    {
        public PlanProfile()
        {
            CreateMap<PlanRequestDto, Plan>();
            CreateMap<Plan, PlanResponseDto>();
        }
    }
}
