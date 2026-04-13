using ProjectSummary.Models.Enums;

namespace ProjectSummary.Models.Requests
{
    public class UpdateStatusRequest
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
