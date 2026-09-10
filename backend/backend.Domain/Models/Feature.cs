namespace backend.Domain.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Icon { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<PrivilegeFeature> PrivilegeFeatures { get; set; } = new List<PrivilegeFeature>();
    }
}
