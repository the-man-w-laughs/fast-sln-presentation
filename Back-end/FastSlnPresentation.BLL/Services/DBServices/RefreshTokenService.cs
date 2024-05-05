using AutoMapper;
using FastSlnPresentation.DAL.DBContext;
using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastSlnPresentation.BLL.Services.DBServices
{
    public class RefreshTokenService
    {
        private readonly FastSlnPresentationDbContext _context;
        private readonly IMapper _mapper;

        public RefreshTokenService(FastSlnPresentationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RefreshToken> GenerateTokenAsync(int userId)
        {
            var token = new RefreshToken
            {
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow.ToLocalTime()
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<RefreshToken> GetTokenAsync(int tokenId)
        {
            return await _context.RefreshTokens.FindAsync(tokenId);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(int userId, string token)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(
                t => t.UserId == userId && t.Token == token
            );

            return existingToken;
        }

        public async Task RevokeTokenAsync(int tokenId)
        {
            var token = await GetTokenAsync(tokenId);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RevokeUserTokensAsync(int userId)
        {
            var userTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(userTokens);
            await _context.SaveChangesAsync();
        }
    }
}
