using EisRoutingService.Messages;

namespace EisRoutingService.Contracts
{
    public class BookCreated : IMessage
    {
        public Guid Id { get; set; }
        public string Title { get; set;} = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal InventoryAmount { get; set;}
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
