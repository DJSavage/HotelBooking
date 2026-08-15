using HotelBooking.Application.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

//documentation: This class implements the IBookingRepository interface and provides methods for accessing booking data from the database.

namespace HotelBooking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        //documentation: The BookingRepository class is responsible for managing booking data in the application.
        //It provides methods to retrieve and add bookings to the database using Entity Framework Core.
        //The class interacts with the BookingDbContext to perform database operations related to bookings, including retrieving bookings by reference and adding new bookings.
        private readonly BookingDbContext _db;

        public BookingRepository(BookingDbContext db)
        {
            _db = db;
        }

        //documentation: This method retrieves a booking by its reference from the database.
        public Task<Booking?> GetByReferenceAsync(string reference)
            => _db.Bookings
                  .Include(b => b.Room)
                  .ThenInclude(r => r.Hotel)
                  .FirstOrDefaultAsync(b => b.BookingReference == reference);

        //documentation: This method adds a new booking to the database.
        public async Task<Booking?> CreateBookingAsync(Booking booking)
        {
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return await _db.Bookings
        .Include(b => b.Room)
        .ThenInclude(r => r.Hotel)
        .FirstAsync(b => b.Id == booking.Id);
        }
    }
}
