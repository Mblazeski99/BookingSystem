using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Models
{
    public class SearchRequest
    {
        [Required]
        public string Destination { get; set; }

        public string DepartureAirport { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public override string ToString()
        {
            return $"{Destination}-{DepartureAirport}-{FromDate}-{ToDate}";
        }
    }
}