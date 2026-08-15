namespace HotelBooking.Domain.Entities
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int MaxGuests { get; set; }
        public decimal BasePrice { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
