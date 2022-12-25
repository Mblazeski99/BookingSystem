using BookingSystem.Domain.Models.Enums;

namespace BookingSystem.Domain.Models
{
    public class BookingItem
    {
        public string BookingCode { get; set; }
        public int SleepTimeSeconds { get; set; }
        public DateTime BookingTime { get; set; }
        public SearchType SearchType { get; set;}
    }
}
