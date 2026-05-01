namespace ProjectSummary.Models.Requests
{
    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
} 
