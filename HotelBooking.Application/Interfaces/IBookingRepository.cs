using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByReferenceAsync(string reference);
        Task AddAsync(Booking booking);
    }
}
