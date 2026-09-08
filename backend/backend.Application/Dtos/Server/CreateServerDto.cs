namespace backend.Application.Dtos.Server
{
    public class CreateServerDto
    {
        public string IpAddress { get; set; } = string.Empty;
        public ushort Port { get; set; }
        public bool IsActive { get; set; }
    }
}
