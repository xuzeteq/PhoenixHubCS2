namespace backend.Application.Dtos.Privilege
{
    public class PrivilegeResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
