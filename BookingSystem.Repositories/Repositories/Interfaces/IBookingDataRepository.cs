using BookingSystem.DataAccess;
using BookingSystem.Domain.Models.Interfaces;

namespace BookingSystem.Repositories.Repositories.Interfaces
{
    public interface IBookingDataRepository
    {
        Task<DataStoreOperationResponse<IModelResponse>> InsertResponse(IModelResponse responseItem);
    }
}
