using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingService _bookingService;

        public BookingService(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public Task<Booking?> GetByReferenceAsync(string reference)
        => _bookingService.GetByReferenceAsync(reference);

        public Task<Booking> CreateBookingAsync(int RoomId, DateOnly StartDate, DateOnly EndDate, int Guests)
        => _bookingService.CreateBookingAsync(RoomId, StartDate, EndDate, Guests);
    }
}
