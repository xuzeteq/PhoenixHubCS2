using backend.Domain.Enums.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("steam_id")]
        public string SteamId { get; set; } = string.Empty;

        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Column("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [Column("balance")]
        public decimal Balance { get; set; }

        [Column("role")]
        public RoleEnum Role { get; set; } = RoleEnum.Пользователь;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("last_login_at")]
        public DateTime LastLoginAt { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry")]
        public DateTime? RefreshTokenExpiry { get; set; }

        [Column("subscribtion_expire_at")]
        public DateTime? SubscribtionExpireAt { get; set; }

        [Column("is_verify")]
        public bool IsVerify { get; set; }
    }
}
