namespace HotelBooking.Api.Models
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RoomDto> Rooms { get; set; } = new();
    }
}
