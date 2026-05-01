using ProjectSummary.Models.Requests;

namespace ProjectSummary.Services.Interfaces
{
    public interface ICartService
    {
        Task<object> GetCartAsync(int userId);
        Task<object> AddToCartAsync(AddToCartRequest request);
        Task<object> UpdateQuantityAsync(UpdateQuantityRequest request);
        Task<object> RemoveFromCartAsync(RemoveFromCartRequest request);
    }
}
