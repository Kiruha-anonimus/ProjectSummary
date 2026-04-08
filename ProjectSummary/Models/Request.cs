namespace ProjectSummary.Models
{
    public class Request
    {
        public class ProductRequest
        {
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public int Price { get; set; }
            public int Quantity { get; set; }
        }

        public class AddToCartRequest
        {
            public int UserId { get; set; } 
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class RemoveFromCartRequest
        {
            public int UserId { get; set; }
            public int ProductId { get; set; }
        }
        public class UpdateQuantityRequest
        {
            public int UserId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
        public class UpdateOrderStatusRequest
        {
            public int OrderId { get; set; }
            public string Status { get; set; }
        }
        public class CreateOrderRequest
        {
            public int UserId { get; set; }
        }
        public class UpdateStatusRequest
        {
            public int OrderId { get; set; }
            public string Status { get; set; }
        }
    }
}
