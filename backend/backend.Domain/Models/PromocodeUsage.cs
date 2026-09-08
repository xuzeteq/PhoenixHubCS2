namespace backend.Domain.Models
{
    public class PromocodeUsage
    {
        public int Id { get; set; }
        public int PromocodeId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Promocode Promocode { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
