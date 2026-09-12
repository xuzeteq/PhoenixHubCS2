using backend.Application.Dtos.Feature;

namespace backend.Application.Dtos.Privilege
{
    public class PrivilegeResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<FeatureResponseDto> Features { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
