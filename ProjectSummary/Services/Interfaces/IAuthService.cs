using ProjectSummary.Models.Requests;
using ProjectSummary.Models.Responses;

public interface IAuthService
{
    Task<BaseResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}