namespace backend.Domain.Models
{
    public class Subscribtion
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }

        public User User { get; set; } = null!;
    }
}
