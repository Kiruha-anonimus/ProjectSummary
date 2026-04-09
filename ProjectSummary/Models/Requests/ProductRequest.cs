namespace ProjectSummary.Models.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
