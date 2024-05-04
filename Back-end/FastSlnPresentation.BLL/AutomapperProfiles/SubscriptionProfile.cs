using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.DAL.Models;

namespace FastSlnPresentation.BLL.AutomapperProfiles
{
    public class SubscriptionProfile : BaseProfile
    {
        public SubscriptionProfile()
        {
            CreateMap<SubscriptionRequestDto, Subscription>();
            CreateMap<Subscription, SubscriptionResponseDto>();
        }
    }
}
