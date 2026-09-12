using backend.Application.Dtos.Feature;

namespace backend.Application.Dtos.Privilege
{
    public class UpdatePrivilegeDto
    {
        public string? Title { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<int>? Features { get; set; } 
        public decimal? OldPrice { get; set; }
    }
}
