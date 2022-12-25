using BookingSystem.Domain.Models.Enums;

namespace BookingSystem.Domain.Models
{
    public class Option
    {
        public string OptionCode { get; set; }
        public string HotelCode { get; set; }
        public string FlightCode { get; set; }
        public string ArrivalAirport { get; set; }
        public double Price { get; set; }
        public SearchType SearchType { get; set; }
    }
}