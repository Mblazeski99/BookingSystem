using BookingSystem.Domain.Models;

namespace BookingSystem.Services.Interfaces
{
    public interface IManagerService
    {
        Task<SearchResponse> Search(SearchRequest request);
        Task<BookResponse> Book(BookRequest request);
        Task<CheckStatusResponse> CheckStatus(CheckStatusRequest request);
    }
}
