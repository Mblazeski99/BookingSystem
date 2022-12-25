using System.Text.Json.Serialization;

namespace BookingSystem.DataAccess.Models
{
    public class FlightDataItem
    {
        [JsonPropertyName("flightCode")]
        public string FlightCode { get; set; }

        [JsonPropertyName("flightNumber")]
        public string FlightNumber { get; set; }

        [JsonPropertyName("departureAirport")]
        public string DepartureAirport { get; set; }

        [JsonPropertyName("arrivalAirport")]
        public string ArrivalAirport { get; set; }
    }
}
