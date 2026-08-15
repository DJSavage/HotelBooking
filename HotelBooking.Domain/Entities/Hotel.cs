using System;
using System.Collections.Generic;
using System.Text;

//documentation: This class represents a hotel entity in the application, which contains information about the hotel and its rooms.
namespace HotelBooking.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<Room> Rooms { get; set; } = new();
    }
}
