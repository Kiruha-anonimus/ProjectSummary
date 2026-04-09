namespace ProjectSummary.Models.Requests
{
    public class RemoveFromCartRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
