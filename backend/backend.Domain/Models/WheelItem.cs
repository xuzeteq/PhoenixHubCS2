using backend.Domain.Enums;

namespace backend.Domain.Models
{
    public class WheelItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int Weight { get; set; }
        public string Rarity { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
