using HotelBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;
        public List<Booking> Bookings { get; set; } = new();
    }
}
