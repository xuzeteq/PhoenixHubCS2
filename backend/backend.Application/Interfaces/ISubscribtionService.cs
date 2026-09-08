using backend.Application.Dtos.Subscribtion;

namespace backend.Application.Interfaces
{
    public interface ISubscribtionService
    {
        Task<List<SubscribtionResponseDto>> GetAllSubscribtionsAsync(CancellationToken cts = default);
        Task PurchaseSubscribeAsync(int userId, CancellationToken cts = default);
    }
}
