namespace ProjectSummary.Models.Requests
{
    public class UpdateStatusRequest
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
    }
}
