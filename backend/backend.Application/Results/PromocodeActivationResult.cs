using backend.Domain.Models;

namespace backend.Application.Results
{
    public abstract record PromocodeActivationResult
    {
        public sealed record Success(Promocode promocode) : PromocodeActivationResult;
        public sealed record NotFound() : PromocodeActivationResult;
        public sealed record Expired() : PromocodeActivationResult; 
        public sealed record AlreadyUsed() : PromocodeActivationResult;
        public sealed record UsageLimit() : PromocodeActivationResult;
        public sealed record Error(string message) : PromocodeActivationResult;
    }
}
