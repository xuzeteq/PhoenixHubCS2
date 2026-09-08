using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(User user, out DateTime expiresAt);
        string CreateRefreshToken();
    }
}
