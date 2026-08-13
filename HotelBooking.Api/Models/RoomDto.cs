namespace HotelBooking.Api.Models
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public string RoomType { get; set; } = string.Empty;
    }
}
