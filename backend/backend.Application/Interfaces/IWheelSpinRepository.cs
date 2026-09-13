using backend.Domain.Models;

namespace backend.Application.Interfaces
{
    public interface IWheelSpinRepository
    {
        Task RecordSpinWheelAsync(WheelItem spin);
        Task<bool> HasSpunToday(int userId);
    }
}
