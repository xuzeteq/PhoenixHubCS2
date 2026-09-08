namespace backend.Application.Dtos.Server
{
    public class ServerResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Map { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public ushort Port { get; set; }
        public int Online { get; set; }
        public int MaxOnline { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}