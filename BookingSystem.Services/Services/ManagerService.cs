using BookingSystem.DataAccess;
using BookingSystem.Domain.Models;
using BookingSystem.Domain.Models.Exceptions;
using BookingSystem.Services.Interfaces;

namespace BookingSystem.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IDataStore _bookingSystemDataStore;

        public ManagerService(IDataStore bookingSystemDataStore)
        {
            _bookingSystemDataStore = bookingSystemDataStore;
        }

        public async Task<BookResponse> Book(BookRequest request)
        {
            var response = await _bookingSystemDataStore.Book(request);
            if (response.Success)
            {
                return response.Data;
            }

            throw new CustomDataException(response.Error);
        }

        public async Task<CheckStatusResponse> CheckStatus(CheckStatusRequest request)
        {
            var response = await _bookingSystemDataStore.CheckStatus(request);
            if (response.Success)
            {
                return response.Data;
            }

            throw new CustomDataException(response.Error);
        }

        public async Task<SearchResponse> Search(SearchRequest request)
        {
            var response = await _bookingSystemDataStore.Search(request);
            if (response.Success)
            {
                return response.Data;
            }

            throw new CustomDataException(response.Error);
        }
    }
}
