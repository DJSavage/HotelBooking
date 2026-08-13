namespace HotelBooking.Api.Models
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Guests { get; set; }
    }
}
