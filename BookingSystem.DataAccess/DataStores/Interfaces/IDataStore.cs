using BookingSystem.Domain.Models;

namespace BookingSystem.DataAccess
{
    public interface IDataStore
    {
        Task<DataStoreOperationResponse<BookResponse>> Book(BookRequest bookRequest);
        
        Task<DataStoreOperationResponse<CheckStatusResponse>> CheckStatus(CheckStatusRequest checkStatusRequest);

        Task<DataStoreOperationResponse<SearchResponse>> Search(SearchRequest searchRequest);
    }
}
