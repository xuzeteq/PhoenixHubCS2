using backend.Application.Dtos.Subscribtion;
using backend.Domain.Models;

namespace backend.Application.Mappings
{
    public static class SubscribtionMapping
    {
        public static SubscribtionResponseDto ToDto(this Subscribtion subscribtion)
        {
            return new SubscribtionResponseDto
            {
                Id = subscribtion.Id,
                UserId = subscribtion.UserId,
                CreatedAt = subscribtion.CreatedAt,
                ExpireAt = subscribtion.ExpireAt
            };
        }
    }
}
