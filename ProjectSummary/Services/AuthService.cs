using Microsoft.EntityFrameworkCore;
using ProjectSummary.Data;
using ProjectSummary.Models.Entities;
using ProjectSummary.Models.Requests;
using ProjectSummary.Models.Responses;
using ProjectSummary.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenService _tokenService;

    public AuthService(AppDbContext context, JwtTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<BaseResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Пользователь уже существует"
            };
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new BaseResponse
        {
            Success = true,
            Message = "Регистрация успешна"
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || user.Password != request.Password)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Неверный email или пароль"
            };
        }

        var token = _tokenService.GenerateToken(user.Id, user.Username, user.Email);

        return new LoginResponse
        {
            Success = true,
            Message = "Успешный вход",
            Token = token
        };
    }
}