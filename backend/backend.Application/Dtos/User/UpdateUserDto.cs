using backend.Domain.Enums.User;

namespace backend.Application.Dtos.User
{
    public class UpdateUserDto
    {
        public decimal? Balance { get; set; }
        public RoleEnum? Role { get; set; }
        public DateTime? SubscribtionExpireAt { get; set; }
        public bool IsVerify { get; set; }
    }
}
