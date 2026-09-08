namespace backend.Domain.Models
{
    public class Promocode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal GiveBalance { get; set; }
        public int UsedCount { get; set; }
        public int MaxUses { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }

        public ICollection<PromocodeUsage> Usages = new List<PromocodeUsage>();
    }
}
