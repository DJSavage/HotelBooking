namespace HotelBooking.Api.Models
{
    //documentation: This class represents a Data Transfer Object (DTO) for creating a new booking.
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Guests { get; set; }
    }
}
