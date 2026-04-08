using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSummary.Data;
using ProjectSummary.Models;
using static ProjectSummary.Models.Request;

namespace ProjectSummary.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // Список всех продуктов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // Добавить продукт
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Пустой запрос" });

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Продукт добавлен"
            });
        }

        // Изменить продукт
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Пустой запрос" });

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new { success = false, message = "Продукт не найден" });

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Продукт изменён"
            });
        }

        // Удалить продукт
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new { success = false, message = "Продукт не найден" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Продукт удалён"
            });
        }
    }
}