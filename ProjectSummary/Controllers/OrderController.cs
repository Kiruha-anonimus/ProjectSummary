using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSummary.Data;
using ProjectSummary.Models;
using ProjectSummary.Models.Enums;
using ProjectSummary.Models.DTOs;
using static ProjectSummary.Models.Request;

namespace ProjectSummary.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // Получить все заказы
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.OrderItems
                .Select(x => new OrderItemDto
                {
                    OrderId = x.OrderId,
                    UserId = x.Order.UserId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                })
                .ToListAsync();

            return Ok(items);
        }

        // Получить заказ по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var items = await _context.OrderItems
                .Where(x => x.OrderId == id)
                .Select(x => new OrderItemDto
                {
                    OrderId = x.OrderId,
                    UserId = x.Order.UserId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("Заказ не найден");

            return Ok(items);
        }

        // Создать заказ из корзины
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] int userId)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("Корзина пуста");

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.New
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = cartItems.Select(c => new OrderItem
            {
                OrderId = order.Id,
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Product.Price
            }).ToList();

            _context.OrderItems.AddRange(orderItems);

            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Заказ создан",
                orderId = order.Id
            });
        }

        // Изменение статус заказа по айди заказа
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);

            if (order == null)
                return NotFound("Заказ не найден");

            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
                return BadRequest("Неверный статус");

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok("Статус обновлен");
        }
    }

}