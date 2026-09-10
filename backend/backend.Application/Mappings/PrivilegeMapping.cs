using backend.Application.Dtos.Privilege;
using backend.Domain.Models;

namespace backend.Application.Mappings
{
    public static class PrivilegeMapping
    {
        public static PrivilegeResponseDto ToDto(this Privilege privilege)
        {
            return new PrivilegeResponseDto
            {
                Id = privilege.Id,
                Title = privilege.Title,
                Price = privilege.Price,
                OldPrice = privilege.OldPrice,
                CreatedAt = privilege.CreatedAt,
            };
        }
    }
}
