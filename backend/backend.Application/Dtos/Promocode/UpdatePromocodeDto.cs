namespace backend.Application.Dtos.Promocode
{
    public class UpdatePromocodeDto
    {
        public string? Code { get; set; }
        public decimal? GiveBalance { get; set; }
        public int? MaxUses { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
