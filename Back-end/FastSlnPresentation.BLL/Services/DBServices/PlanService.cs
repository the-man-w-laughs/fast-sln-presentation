using AutoMapper;
using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.DAL.DBContext;
using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastSlnPresentation.BLL.Services.DBServices
{
    public class PlanService
    {
        private readonly FastSlnPresentationDbContext _context;
        private readonly IMapper _mapper;

        public PlanService(FastSlnPresentationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PlanResponseDto>> GetAllPlans()
        {
            var plans = await _context.Plans.ToListAsync();

            var planResponseDtos = _mapper.Map<List<PlanResponseDto>>(plans);

            return planResponseDtos;
        }

        public async Task<PlanResponseDto> DeletePlanIfNoActiveOrFutureSubscriptions(int planId)
        {
            // Найдите план по его идентификатору
            var plan = await _context.Plans.FindAsync(planId);

            if (plan == null)
            {
                throw new ArgumentException($"План с id {planId} не найден.");
            }

            // Проверить наличие активных или будущих подписок на этот план
            bool hasActiveOrFutureSubscriptions = await _context.Subscriptions.AnyAsync(
                s => s.PlanId == planId && s.EndDate >= DateTime.UtcNow
            );

            if (hasActiveOrFutureSubscriptions)
            {
                throw new InvalidOperationException(
                    $"План с id {planId} не может быть удален, так как у него есть активные или будущие подписки."
                );
            }

            _context.Plans.Remove(plan);
            var planResponseDto = _mapper.Map<PlanResponseDto>(plan);
            await _context.SaveChangesAsync();

            return planResponseDto;
        }

        public async Task<PlanResponseDto> CreatePlan(PlanRequestDto planRequestDto)
        {
            bool planExists = await _context.Plans.AnyAsync(p => p.Name == planRequestDto.Name);

            if (planExists)
            {
                throw new ArgumentException(
                    $"План с названием '{planRequestDto.Name}' уже существует."
                );
            }

            var plan = _mapper.Map<Plan>(planRequestDto);
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();
            var planResponseDto = _mapper.Map<PlanResponseDto>(plan);

            return planResponseDto;
        }
    }
}
