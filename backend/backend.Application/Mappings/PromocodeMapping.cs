using backend.Application.Dtos.Promocode;
using backend.Domain.Models;

namespace backend.Application.Mappings
{
    public static class PromocodeMapping
    {
        public static PromocodeResponseDto ToDto(this Promocode promocode)
        {
            return new PromocodeResponseDto
            {
                Id = promocode.Id,
                Code = promocode.Code,
                GiveBalance = promocode.GiveBalance,
                UsedCount = promocode.UsedCount,
                MaxUses = promocode.MaxUses,
                CreatedAt = promocode.CreatedAt,
                ExpireAt = promocode.ExpireAt,
            };
        }
    }
}
