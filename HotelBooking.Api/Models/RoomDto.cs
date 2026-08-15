namespace HotelBooking.Api.Models
{
    //documentation: This class represents a Data Transfer Object (DTO) for room information.
    public class RoomDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public int RoomTypeId { get; set; }
    }
}
