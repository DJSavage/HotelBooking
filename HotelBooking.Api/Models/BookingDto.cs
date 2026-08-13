namespace HotelBooking.Api.Models
{
    public class BookingDto
    {
        public string BookingReference { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Guests { get; set; }
        public int RoomId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
    }
}
