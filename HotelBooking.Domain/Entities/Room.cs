using HotelBooking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }

        public int Number { get; set; }        // ← Add this

        public RoomType RoomType { get; set; } // ← Rename this

        public int Capacity { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public List<Booking> Bookings { get; set; } = new();
    }

}
