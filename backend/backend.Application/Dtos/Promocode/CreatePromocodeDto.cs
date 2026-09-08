namespace backend.Application.Dtos.Promocode
{
    public class CreatePromocodeDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal GiveBalance { get; set; }
        public int MaxUses { get; set; }
        public int DaysActive { get; set; }
    }
}
