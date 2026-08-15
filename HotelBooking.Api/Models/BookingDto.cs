namespace HotelBooking.Api.Models
{
    //documentation: This class represents a Data Transfer Object (DTO) for booking information.
    //It is used to transfer booking data between the API and clients, encapsulating details such as booking reference, dates, number of guests, room ID, hotel name, and room type.
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
