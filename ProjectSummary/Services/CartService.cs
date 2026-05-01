using Microsoft.EntityFrameworkCore;
using ProjectSummary.Data;
using ProjectSummary.Models;
using ProjectSummary.Models.Requests;
using ProjectSummary.Services.Interfaces;

namespace ProjectSummary.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetCartAsync(int userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    productId = c.ProductId,
                    productName = c.Product.Name,
                    price = c.Product.Price,
                    quantity = c.Quantity
                })
                .ToListAsync();
        }

        public async Task<object> AddToCartAsync(AddToCartRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return new { success = false, message = "Товар не найден" };

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

            if (existingItem != null)
                existingItem.Quantity += request.Quantity;
            else
                _context.CartItems.Add(new CartItem
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });

            await _context.SaveChangesAsync();

            return new { success = true, message = "Товар добавлен" };
        }

        public async Task<object> UpdateQuantityAsync(UpdateQuantityRequest request)
        {
            var cartItem = await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c =>
                    c.UserId == request.UserId &&
                    c.ProductId == request.ProductId);

            if (cartItem == null)
                return new { success = false, message = "Товар не найден" };

            cartItem.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return new { success = true, message = "Обновлено" };
        }

        public async Task<object> RemoveFromCartAsync(RemoveFromCartRequest request)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.UserId == request.UserId &&
                    c.ProductId == request.ProductId);

            if (cartItem == null)
                return new { success = false, message = "Товар не найден" };

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return new { success = true, message = "Удалено" };
        }
    }
}