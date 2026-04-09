using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSummary.Data;
using ProjectSummary.Models;
using ProjectSummary.Models.Requests;

namespace ProjectSummary.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Получить корзину по userId
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var items = await _context.CartItems
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

            return Ok(items);
        }

        // Добавить товар в корзину
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Пустой запрос" });

            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null)
                return BadRequest(new { success = false, message = "Товар не найден" });

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    Quantity = 1
                };

                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Товар добавлен в корзину"
            });
        }

        // Изменить количество товара в корзине
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Пустой запрос" });

            var cartItem = await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c =>
                    c.UserId == request.UserId &&
                    c.ProductId == request.ProductId);

            if (cartItem == null)
                return NotFound(new { success = false, message = "Товар в корзине не найден" });

            if (request.Quantity > cartItem.Product.Quantity)
                return BadRequest(new
                {
                    success = false,
                    message = $"В наличии только: {cartItem.Product.Quantity}"
                });

            cartItem.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Количество обновлено"
            });
        }

        // Удалить товар из корзины
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Пустой запрос" });

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                    c.UserId == request.UserId &&
                    c.ProductId == request.ProductId);

            if (cartItem == null)
                return NotFound(new { success = false, message = "Товар не найден в корзине" });

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Товар успешно удален из корзины"
            });
        }
    }
}