namespace backend.Application.Dtos.Feature
{
    public class CreateFeatureDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Icon { get; set; } = string.Empty;
    }
}
