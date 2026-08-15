using System;
using System.Collections.Generic;
using System.Text;

//documentation: This class represents a hotel room entity in the domain layer of the application.
//It contains properties that define the characteristics of a room, such as its number, type, capacity, and associated hotel.
//It also maintains a list of bookings associated with the room.
namespace HotelBooking.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = default!;

        public int Capacity { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public List<Booking> Bookings { get; set; } = new();
    }

}
