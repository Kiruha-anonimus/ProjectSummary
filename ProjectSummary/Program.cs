using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectSummary.Data;
using ProjectSummary.Services;
using ProjectSummary.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Сервисы приложения
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();

// Подключение к бд
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Единая настройка JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],

        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };
});

builder.Services.AddAuthorization();

// Регистрируем сервисы
builder.Services.AddSingleton<JwtTokenService>();        // добавить интерфейс

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();