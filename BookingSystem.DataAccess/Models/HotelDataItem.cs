using System.Text.Json.Serialization;

namespace BookingSystem.DataAccess.Models
{
    public class HotelDataItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("hotelCode")]
        public string HotelCode { get; set; }

        [JsonPropertyName("hotelName")]
        public string HotelName { get; set; }

        [JsonPropertyName("destinationCode")]
        public string DestinationCode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }
    }
}
