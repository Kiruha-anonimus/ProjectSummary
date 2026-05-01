using Microsoft.AspNetCore.Mvc;
using ProjectSummary.Services.Interfaces;
using ProjectSummary.Models.Requests;

namespace ProjectSummary.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var result = await _cartService.GetCartAsync(userId);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var result = await _cartService.AddToCartAsync(request);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var result = await _cartService.UpdateQuantityAsync(request);
            return Ok(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            var result = await _cartService.RemoveFromCartAsync(request);
            return Ok(result);
        }
    }
}