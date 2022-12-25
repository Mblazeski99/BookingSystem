using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Models
{
    public class BookRequest
    {
        [Required]
        public string OptionCode { get; set; }

        [Required]
        public SearchRequest SearchRequest { get; set; }
    }
}
