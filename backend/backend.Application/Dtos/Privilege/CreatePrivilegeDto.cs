namespace backend.Application.Dtos.Privilege
{
    public class CreatePrivilegeDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<int> FeatureIds { get; set; } = new();
    }
}
