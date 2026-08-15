using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> GetByReferenceAsync(string reference);

        Task<Booking?> CreateBookingAsync(int RoomId, DateOnly StartDate, DateOnly EndDate, int Guests);
    }
}
