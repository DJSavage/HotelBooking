using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

//documentation: This interface defines the contract for a booking repository, which is responsible for managing hotel bookings in the application.
//It provides methods to retrieve a booking by its reference and to create a new booking.
namespace HotelBooking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByReferenceAsync(string reference);
        Task<Booking?> CreateBookingAsync(Booking booking);
    }
}
