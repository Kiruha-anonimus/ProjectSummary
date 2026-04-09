namespace ProjectSummary.Models.Responses
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponse> Items { get; set; }
    }
}
