namespace backend.Application.Dtos.Promocode
{
    public class PromocodeResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal GiveBalance { get; set; }
        public int UsedCount { get; set; }
        public int MaxUses { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
