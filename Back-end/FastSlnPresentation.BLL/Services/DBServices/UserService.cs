using AutoMapper;
using FastSlnPresentation.BLL.DTOs;
using FastSlnPresentation.BLL.Exceptions;
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

        public async Task<UserResponseDto> CreateUser(UserRequestDto userRequestDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(
                user => user.Email == userRequestDto.Email
            );

            if (existingUser != null)
            {
                throw new ArgumentException(
                    $"Пользователь с email \"{userRequestDto.Email}\" уже существует."
                );
            }

            if (string.IsNullOrWhiteSpace(userRequestDto.Password))
            {
                throw new ArgumentException("Пароль не может быть пустым.");
            }

            var (passwordHash, salt) = PasswordHashingService.HashPassword(userRequestDto.Password);

            var user = new User
            {
                Name = userRequestDto.Name,
                Email = userRequestDto.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                RoleId = userRequestDto.RoleId,
                CreatedAt = DateTime.UtcNow.ToLocalTime(),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return userResponseDto;
        }

        public async Task<UserResponseDto> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с id {id} не найден.");
            }

            if (user.Id == 1)
            {
                throw new InvalidOperationException(
                    $"Невозможно удалить пользователя с id {user.Id}."
                );
            }

            bool hasActiveSubscriptions = user.Subscriptions.Any(
                subscription =>
                    subscription.StartDate.Date <= DateTime.UtcNow.Date
                    && subscription.EndDate.Date >= DateTime.UtcNow.Date
            );

            if (hasActiveSubscriptions)
            {
                throw new InvalidOperationException(
                    "Невозможно удалить пользователя с активными подписками."
                );
            }

            bool hasFutureSubscriptions = user.Subscriptions.Any(
                subscription => subscription.StartDate.Date > DateTime.UtcNow.Date
            );

            if (hasFutureSubscriptions)
            {
                throw new InvalidOperationException(
                    "Невозможно удалить пользователя с подписками, начинающимися позже сегодняшнего дня."
                );
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return userResponseDto;
        }

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var result = _mapper.Map<List<UserResponseDto>>(users);

            return result;
        }

        public async Task<UserResponseDto> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"Пользователь с id {id} не найден.");
            }

            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return userResponseDto;
        }
    }
}
