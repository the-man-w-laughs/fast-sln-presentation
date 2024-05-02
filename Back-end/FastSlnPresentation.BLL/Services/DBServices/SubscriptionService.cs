using AutoMapper;
using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.DAL.DBContext;
using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastSlnPresentation.BLL.Services.DBServices
{
    public class SubscriptionService
    {
        private readonly FastSlnPresentationDbContext _context;
        private readonly IMapper _mapper;

        public SubscriptionService(FastSlnPresentationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SubscriptionResponseDto> CreateSubscription(
            SubscriptionRequestDto subscriptionRequestDto
        )
        {
            // Validate the provided user ID
            var user = await _context.Users.FindAsync(subscriptionRequestDto.UserId);

            if (user == null)
            {
                throw new ArgumentException(
                    $"User with ID {subscriptionRequestDto.UserId} not found."
                );
            }

            // Validate the provided plan ID
            var plan = await _context.Plans.FindAsync(subscriptionRequestDto.PlanId);

            if (plan == null)
            {
                throw new ArgumentException(
                    $"Plan with ID {subscriptionRequestDto.PlanId} not found."
                );
            }

            var subscription = _mapper.Map<Subscription>(subscriptionRequestDto);
            subscription.EndDate = subscription.StartDate.AddDays(plan.Duration);

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<SubscriptionResponseDto>(subscription);

            return result;
        }

        public async Task<List<SubscriptionResponseDto>> GetUserSubscriptions(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new ArgumentException($"Пользователь с id {userId} не найден.");
            }

            var subscriptions = await _context.Subscriptions
                .Where(s => s.UserId == userId)
                .ToListAsync();

            var subscriptionResponseDtos = _mapper.Map<List<SubscriptionResponseDto>>(
                subscriptions
            );

            return subscriptionResponseDtos;
        }

        public async Task<SubscriptionResponseDto> GetActiveSubscription(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new ArgumentException($"Пользователь с id {userId} не найден.");
            }

            var oldestActiveSubscription = await _context.Subscriptions
                .Where(
                    s =>
                        s.UserId == userId
                        && s.StartDate.Date <= DateTime.UtcNow.Date
                        && s.EndDate.Date >= DateTime.UtcNow.Date
                )
                .OrderBy(s => s.StartDate)
                .FirstOrDefaultAsync();

            if (oldestActiveSubscription == null)
            {
                return null;
            }

            var subscriptionResponseDto = _mapper.Map<SubscriptionResponseDto>(
                oldestActiveSubscription
            );

            return subscriptionResponseDto;
        }

        public async Task<SubscriptionResponseDto> DeleteSubscription(int subscriptionId)
        {
            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);

            if (subscription == null)
            {
                throw new ArgumentException($"Подписка с id {subscriptionId} не найдена.");
            }

            var subscriptionResponseDto = _mapper.Map<SubscriptionResponseDto>(subscription);
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return subscriptionResponseDto;
        }
    }
}
