using backend.Domain.Models;

namespace backend.Application.Results
{
    public abstract record WheelSpinResult
    {
        public sealed record Success(WheelItem spin) : WheelSpinResult;
        public sealed record AlreadySpun() : WheelSpinResult;
        public sealed record Error(string message) : WheelSpinResult;
    }
}
