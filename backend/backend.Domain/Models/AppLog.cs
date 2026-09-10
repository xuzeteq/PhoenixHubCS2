namespace backend.Domain.Models
{
    public class AppLog
    {
        public string Message { get; set; } = string.Empty;
        public string MessageTemplate { get; set; } = string.Empty;
        public int Level { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
    }
}
