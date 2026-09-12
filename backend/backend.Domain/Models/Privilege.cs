namespace backend.Domain.Models
{
    public class Privilege
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<PrivilegeFeature> PrivilegeFeatures { get; set; } = new List<PrivilegeFeature>();
    }
}
