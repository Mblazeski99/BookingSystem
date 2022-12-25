namespace BookingSystem.Domain.Models.Exceptions
{
    public class CustomDataException : Exception
    {
        public string CustomErrorMessage { get; set; }

        public CustomDataException(string message) : base(message) 
        {
            CustomErrorMessage = message;
        }
    }
}
