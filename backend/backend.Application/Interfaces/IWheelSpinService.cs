using backend.Application.Results;

namespace backend.Application.Interfaces
{
    public interface IWheelSpinService
    {
        Task<WheelSpinResult> SpinAsync(int userId);
    }
}
