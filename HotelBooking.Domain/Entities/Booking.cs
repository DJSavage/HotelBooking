using System;
using System.Collections.Generic;
using System.Text;

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
