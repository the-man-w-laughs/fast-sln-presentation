using AutoMapper;
using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Exceptions;
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
                throw new NotFoundException(
                    $"Пользователь с id {subscriptionRequestDto.UserId} не найден."
                );
            }

            // Validate the provided plan ID
            var plan = await _context.Plans.FindAsync(subscriptionRequestDto.PlanId);

            if (plan == null)
            {
                throw new NotFoundException(
                    $"План с id {subscriptionRequestDto.PlanId} не найден."
                );
            }

            var subscription = _mapper.Map<Subscription>(subscriptionRequestDto);
            subscription.EndDate = subscription.StartDate.AddDays(plan.Duration);
            subscription.CreatedAt = DateTime.UtcNow;

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
                throw new NotFoundException($"Пользователь с id {userId} не найден.");
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
                throw new NotFoundException($"Пользователь с id {userId} не найден.");
            }

            var oldestActiveSubscription = await _context.Subscriptions
                .Where(
                    s =>
                        s.UserId == userId
                        && s.StartDate <= DateTimeOffset.UtcNow
                        && s.EndDate >= DateTimeOffset.UtcNow
                )
                .OrderBy(s => s.StartDate)
                .FirstOrDefaultAsync();

            if (oldestActiveSubscription == null)
            {
                throw new NotFoundException($"У пользователя с id {userId} нет активных подписок.");
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
                throw new NotFoundException($"Подписка с id {subscriptionId} не найдена.");
            }

            var subscriptionResponseDto = _mapper.Map<SubscriptionResponseDto>(subscription);
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return subscriptionResponseDto;
        }
    }
}
