using System;
using System.Collections.Generic;
using System.Text;

//documentation: This class represents a booking entity in the hotel booking domain.
//It contains properties for the booking's unique identifier, reference, associated room, start and end dates, and number of guests.
namespace HotelBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingReference { get; set; } = default!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = default!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
