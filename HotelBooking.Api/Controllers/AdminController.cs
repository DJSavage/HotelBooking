using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


//documentation: This controller provides administrative endpoints for managing the hotel booking database, including resetting and seeding the database with initial data.
namespace HotelBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly BookingDbContext _db;

        public AdminController(BookingDbContext context)
        {
            _db = context;
        }

        // POST: api/admin/reset
        [HttpPost("reset")]
        public async Task<IActionResult> ResetDatabase()
        {
            // Remove all data
            _db.Bookings.RemoveRange(_db.Bookings);
            _db.Rooms.RemoveRange(_db.Rooms);
            _db.Hotels.RemoveRange(_db.Hotels);

            await _db.SaveChangesAsync();

            return Ok("Database reset successfully.");
        }

        // POST: api/admin/seed
        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase()
        {
            if (await _db.Hotels.AnyAsync())
                return BadRequest("Database already contains data. Reset first.");

            var hotels = new List<Hotel>
{
    new Hotel
    {
        Name = "Grand Hotel",
        Rooms = new List<Room>
        {
            new Room { Number = 101, RoomTypeId = 1, Capacity = 1 },
            new Room { Number = 102, RoomTypeId = 2, Capacity = 2 },
            new Room { Number = 103, RoomTypeId = 3, Capacity = 4 },
            new Room { Number = 201, RoomTypeId = 3, Capacity = 4 },
            new Room { Number = 202, RoomTypeId = 2, Capacity = 2 },
            new Room { Number = 203, RoomTypeId = 1, Capacity = 1 },
        }
    },

    new Hotel
    {
        Name = "Ocean View Resort",
        Rooms = new List<Room>
        {
            new Room { Number = 111, RoomTypeId = 1, Capacity = 1 },
            new Room { Number = 112, RoomTypeId = 2, Capacity = 2 },
            new Room { Number = 113, RoomTypeId = 3, Capacity = 4 },
            new Room { Number = 211, RoomTypeId = 1, Capacity = 1 },
            new Room { Number = 212, RoomTypeId = 2, Capacity = 2 },
            new Room { Number = 213, RoomTypeId = 3, Capacity = 4 },
        }
    },

    new Hotel
    {
        Name = "Mountain Lodge",
        Rooms = new List<Room>
        {
            new Room { Number = 310, RoomTypeId = 1, Capacity = 1 },
            new Room { Number = 311, RoomTypeId = 2, Capacity = 2 },
            new Room { Number = 312, RoomTypeId = 3, Capacity = 4 },
            new Room { Number = 410, RoomTypeId = 1, Capacity = 1 },
            new Room { Number = 411, RoomTypeId = 2, Capacity = 2 },
            new Room { Number = 412, RoomTypeId = 3, Capacity = 4 },
        }
    }
};

            _db.Hotels.AddRange(hotels);
            await _db.SaveChangesAsync();

            return Ok("Database seeded successfully.");
        }
    }
}
