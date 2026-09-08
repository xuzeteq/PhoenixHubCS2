using backend.Application.Dtos.Auth;
using backend.Application.Dtos.User;
using backend.Domain.Models;

namespace backend.Application.Mappings
{
    public static class UserMapping
    {
        public static UserMeDto ToMeDto(this User user)
        {
            return new UserMeDto
            {
                Id = user.Id,
                Username = user.Username,
                SteamId = user.SteamId,
                AvatarUrl = user.AvatarUrl,
                Balance = user.Balance,
                Role = user.Role,
                IsVerify = user.IsVerify,
                SubscribtionExpireAt = user.SubscribtionExpireAt,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        public static UserResponseDto ToDto(this User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                SteamId = user.SteamId,
                AvatarUrl = user.AvatarUrl,
                Balance = user.Balance,
                Role = user.Role,
                IsVerify = user.IsVerify,
                SubscribtionExpireAt = user.SubscribtionExpireAt,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}
