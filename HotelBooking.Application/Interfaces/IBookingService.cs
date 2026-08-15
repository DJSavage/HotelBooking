using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

//documentation: This interface defines the contract for a booking service, which is responsible for managing hotel bookings in the application.
namespace HotelBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Booking?> GetByReferenceAsync(string reference);

        Task<Booking?> CreateBookingAsync(int RoomId, DateOnly StartDate, DateOnly EndDate, int Guests);
    }
}
