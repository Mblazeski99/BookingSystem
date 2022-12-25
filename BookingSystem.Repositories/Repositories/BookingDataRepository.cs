using BookingSystem.DataAccess;
using BookingSystem.Domain.Models.Interfaces;
using BookingSystem.Repositories.Repositories.Interfaces;

namespace BookingSystem.Repositories.Repositories
{
    public class BookingDataRepository : IBookingDataRepository
    {
        private readonly BookingSystemDataStore _bookingSystemDataStore;

        public BookingDataRepository(BookingSystemDataStore bookingSystemDataStore)
        {
            _bookingSystemDataStore = bookingSystemDataStore;
        }

        public Task<DataStoreOperationResponse<IModelResponse>> InsertResponse(IModelResponse responseItem)
        {
            return _bookingSystemDataStore.InsertResponseItem(responseItem);
        }
    }
}
