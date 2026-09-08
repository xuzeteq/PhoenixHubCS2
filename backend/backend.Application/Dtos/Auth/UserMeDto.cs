using backend.Domain.Enums.User;

namespace backend.Application.Dtos.Auth
{
    public class UserMeDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string SteamId { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public RoleEnum Role { get; set; }
        public string RoleName => Role.ToString();
        public DateTime? SubscribtionExpireAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsVerify { get; set; }
    }
}
