namespace backend.Application.Dtos.Privilege
{
    public class CreatePrivilegeDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }

        public List<int> FeatureIds { get; set; } = new();
    }
}
