using BookingSystem.DataAccess.Models;
using BookingSystem.Domain.Models;
using BookingSystem.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace BookingSystem.DataAccess
{
    public class BookingSystemDataStore : IDataStore
    {
        private readonly IMemoryCache _memoryCache;

        private readonly List<Option> _options = new List<Option>();
        private readonly List<BookingItem> _bookings = new List<BookingItem>();

        private readonly List<BookResponse> _bookResponses = new List<BookResponse>();
        private readonly List<CheckStatusResponse> _checkStatusResponses = new List<CheckStatusResponse>();
        private readonly List<SearchResponse> _searchResponses = new List<SearchResponse>();

        public BookingSystemDataStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<DataStoreOperationResponse<BookResponse>> Book(BookRequest bookRequest)
        {
            var response = new DataStoreOperationResponse<BookResponse>() { Success = false };

            try
            {
                if (string.IsNullOrEmpty(bookRequest.OptionCode)) 
                {
                    response.Error = "Option Code is a required field!";
                    return response;
                }

                var selectedOption = _options.SingleOrDefault(option => option.OptionCode == bookRequest.OptionCode);
                if (selectedOption == null)
                {
                    response.Error = "Selected option does not exist!";
                    return response;
                }

                Random random = new Random();
                int sleepTime = random.Next(30, 61);

                var bookingItem = new BookingItem()
                {
                    BookingCode = Guid.NewGuid().ToString().Substring(0, 6),
                    SleepTimeSeconds = sleepTime,
                    BookingTime = DateTime.Now,
                    SearchType = selectedOption.SearchType
                };

                var bookingResponse = new BookResponse()
                {
                    BookingCode = bookingItem.BookingCode,
                    BookingTime = bookingItem.BookingTime
                };

                _bookings.Add(bookingItem);
                _bookResponses.Add(bookingResponse);

                response.Data = bookingResponse;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = "Something went wrong, booking was not successfull!";
            }

            return response;
        }

        public async Task<DataStoreOperationResponse<CheckStatusResponse>> CheckStatus(CheckStatusRequest checkStatusRequest)
        {
            var response = new DataStoreOperationResponse<CheckStatusResponse>() { Success = false };

            try
            {
                if (string.IsNullOrEmpty(checkStatusRequest.BookingCode))
                {
                    response.Error = "Booking Code is a required field!";
                    return response;
                }

                var bookingItem = _bookings.FirstOrDefault(b => b.BookingCode == checkStatusRequest.BookingCode);

                if (bookingItem == null)
                {
                    response.Error = "Selected booking does not exist!";
                    return response;
                }

                var result = new CheckStatusResponse();

                bool hasSleepTimeElapsed = DateTime.Now > bookingItem.BookingTime.AddSeconds(bookingItem.SleepTimeSeconds);
                if (hasSleepTimeElapsed)
                {
                    if (bookingItem.SearchType == SearchType.HotelOnly 
                        || bookingItem.SearchType == SearchType.HotelAndFlight)
                    {
                        result.Status = BookingStatusEnum.Success;
                    }
                    
                    if (bookingItem.SearchType == SearchType.LastMinuteHotels)
                    {
                        result.Status = BookingStatusEnum.Failed;
                    }
                }
                else
                {
                    result.Status = BookingStatusEnum.Pending;
                }

                response.Data = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = "Something went wrong, could not get booking status!";
            }

            return response;
        }

        public async Task<DataStoreOperationResponse<SearchResponse>> Search(SearchRequest searchRequest)
        {
            if (_memoryCache.TryGetValue<DataStoreOperationResponse<SearchResponse>>(searchRequest.ToString(), out var cacheResponse))
            {
                return cacheResponse ?? new DataStoreOperationResponse<SearchResponse> { Success = false };
            }

            var result = await SearchHotelsAndFlights(searchRequest);

            if (result.Success)
            {
                _memoryCache.Set(searchRequest.ToString(), result, TimeSpan.FromSeconds(60));
                return result;
            }

            return new DataStoreOperationResponse<SearchResponse>() 
            {
                Success = false, 
                Error = result.Error 
            };
        }

        private async Task<DataStoreOperationResponse<SearchResponse>> SearchHotelsAndFlights(SearchRequest searchRequest)
        {
            var response = new DataStoreOperationResponse<SearchResponse>() { Success = false };

            try
            {
                response.Data = new SearchResponse { Options = new List<Option>() };

                if (string.IsNullOrEmpty(searchRequest.Destination))
                {
                    response.Error = "Destination is a required field!";
                    return response;
                }

                if (searchRequest.FromDate >= searchRequest.ToDate) 
                {
                    response.Error = "Incorrect date input!";
                    return response;
                }

                string hotelsDataUrl = $"https://tripx-test-functions.azurewebsites.net/api/SearchHotels?destinationCode={searchRequest.Destination}";
                string flightsDataUrl = $"https://tripx-test-functions.azurewebsites.net/api/SearchFlights?departureAirport={searchRequest.DepartureAirport}&arrivalAirport={searchRequest.Destination}";

                using var client = new HttpClient();

                var hotelsResponse = await client.GetAsync(hotelsDataUrl);
                var strHotelsData = await hotelsResponse.Content.ReadAsStringAsync();
                List<HotelDataItem> hotelsData = JsonConvert.DeserializeObject<List<HotelDataItem>>(strHotelsData);

                _options.Clear();
                Random random = new Random();

                SearchType searchType = GetSearchRequestSearchType(searchRequest);

                foreach (HotelDataItem item in hotelsData)
                {
                    var option = new Option()
                    {
                        ArrivalAirport = item.DestinationCode,
                        HotelCode = item.HotelCode,
                        OptionCode = item.Id,
                        Price = random.Next(40, 80),
                        SearchType = searchType
                    };

                    _options.Add(option);
                }

                if (searchType == SearchType.HotelAndFlight)
                {
                    var flightsResponse = await client.GetAsync(flightsDataUrl);
                    var strFlightsData = await flightsResponse.Content.ReadAsStringAsync();
                    List<FlightDataItem> flightsData = JsonConvert.DeserializeObject<List<FlightDataItem>>(strFlightsData);

                    foreach (FlightDataItem item in flightsData)
                    {
                        foreach (var option in _options.Where(o => o.ArrivalAirport == item.ArrivalAirport))
                        {
                            option.FlightCode = item.FlightCode;
                        }
                    }
                }

                _options.ForEach(option => response.Data.Options.Add(option));

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = "Something went wrong while searching for hotels or flights, please try again later!";
            }

            return response;
        }
    
        private SearchType GetSearchRequestSearchType(SearchRequest searchRequest)
        {
            SearchType searchType;

            if (string.IsNullOrEmpty(searchRequest.DepartureAirport))
            {
                searchType = SearchType.HotelOnly;
                if (DateTime.Now.AddDays(45) >= searchRequest.FromDate)
                {
                    searchType = SearchType.LastMinuteHotels;
                }
            }
            else
            {
                searchType = SearchType.HotelAndFlight;
            }

            return searchType;
        }
    }
}