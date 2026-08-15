namespace HotelBooking.Api.Models
{
    //documentation: This class represents a Data Transfer Object (DTO) for hotel information.
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RoomDto> Rooms { get; set; } = new();
    }
}
