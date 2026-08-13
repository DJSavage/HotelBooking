using HotelBooking.Application.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;
//documentation: This class implements the IHotelRepository interface and provides methods for accessing hotel data from the database.
// It uses Entity Framework Core to query the database and retrieve hotel information, including related room data.
// The GetHotelByNameAsync method retrieves a hotel by its name, including its associated rooms, and returns it as a Hotel object.

namespace HotelBooking.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly BookingDbContext _dbContext;

        public HotelRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Hotel?>GetHotelByNameAsync(string name)
            => _dbContext.Hotels
                .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Name == name);
    }
}
