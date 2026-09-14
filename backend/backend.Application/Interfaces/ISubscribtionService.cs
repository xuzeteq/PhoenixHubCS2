using backend.Application.Dtos.Subscribtion;
using backend.Application.Results;

namespace backend.Application.Interfaces
{
    public interface ISubscribtionService
    {
        Task<List<SubscribtionResponseDto>> GetAllSubscribtionsAsync(CancellationToken cts = default);
        Task<SubscribtionPurchaseResult> PurchaseSubscribeAsync(int userId, CancellationToken cts = default);
    }
}
