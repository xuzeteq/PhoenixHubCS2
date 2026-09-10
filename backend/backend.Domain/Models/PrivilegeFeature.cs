namespace backend.Domain.Models
{
    public class PrivilegeFeature
    {
        public int Id { get; set; }
        public int PrivilegeId { get; set; }
        public int FeatureId { get; set; }

        public string? Value { get; set; } = string.Empty;

        public Privilege Privilege { get; set; } = null!;
        public Feature Feature { get; set; } = null!;
    }
}
