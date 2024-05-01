using AutoMapper;
using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Services.HardServices.Static;
using FastSlnPresentation.DAL.DBContext;
using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastSlnPresentation.BLL.Services.DBServices
{
    public class UserService
    {
        private readonly FastSlnPresentationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(FastSlnPresentationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserResponseDto?> CheckPassword(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

            if (user == null)
            {
                return null;
            }

            if (!PasswordHashingService.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return null;
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var result = _mapper.Map<List<UserResponseDto>>(users);

            return result;
        }
    }
}
