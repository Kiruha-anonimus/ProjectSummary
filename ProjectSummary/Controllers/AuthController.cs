using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSummary.Data;
using ProjectSummary.Services;
using ProjectSummary.Models.Requests;
using ProjectSummary.Models.Responses;
using ProjectSummary.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectSummary.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _tokenService;

        public AuthController(AppDbContext context, JwtTokenService tokenService)  // сюда тож интерфейс сделать тот же jwt
        {
            _context = context;
            _tokenService = tokenService;
        }

        // api/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([Required][FromBody] RegisterRequest request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest(new BaseResponse
                {
                    Success = false,
                    Message = "Пользователь с такой почтой уже существует"
                });
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new BaseResponse
            {
                Success = true,
                Message = "Пользователь зарегистрирован"
            });
        }

        // api/login
        [HttpPost("login")]
        public IActionResult Login([Required][FromBody] LoginRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null || user.Password != request.Password)
            {
                return Unauthorized(new BaseResponse
                {
                    Success = false,
                    Message = "Неверный email или пароль"
                });
            }

            var token = _tokenService.GenerateToken(user.Id, user.Username, user.Email);

            return Ok(new
            {
                Success = true,
                Message = "Успешный вход",
                Token = token
            });
        }
    }
}