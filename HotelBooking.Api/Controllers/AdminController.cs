using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            new Room { Number = 101, RoomType = RoomType.Single, Capacity = 1 },
            new Room { Number = 102, RoomType = RoomType.Double, Capacity = 2 },
            new Room { Number = 103, RoomType = RoomType.Deluxe, Capacity = 4 },
            new Room { Number = 201, RoomType = RoomType.Deluxe, Capacity = 4 },
            new Room { Number = 202, RoomType = RoomType.Double, Capacity = 2 },
            new Room { Number = 203, RoomType = RoomType.Single, Capacity = 1 },
        }
    },

    new Hotel
    {
        Name = "Ocean View Resort",
        Rooms = new List<Room>
        {
            new Room { Number = 111, RoomType = RoomType.Single, Capacity = 1 },
            new Room { Number = 112, RoomType = RoomType.Double, Capacity = 2 },
            new Room { Number = 113, RoomType = RoomType.Deluxe, Capacity = 3 },
            new Room { Number = 211, RoomType = RoomType.Single, Capacity = 1 },
            new Room { Number = 212, RoomType = RoomType.Double, Capacity = 2 },
            new Room { Number = 213, RoomType = RoomType.Deluxe, Capacity = 3 },

        }
    },

    new Hotel
    {
        Name = "Mountain Lodge",
        Rooms = new List<Room>
        {
            new Room { Number = 310, RoomType = RoomType.Single, Capacity = 1 },
            new Room { Number = 311, RoomType = RoomType.Double, Capacity = 2 },
            new Room { Number = 312, RoomType = RoomType.Deluxe, Capacity = 4 },
            new Room { Number = 410, RoomType = RoomType.Single, Capacity = 1 },
            new Room { Number = 411, RoomType = RoomType.Double, Capacity = 2 },
            new Room { Number = 412, RoomType = RoomType.Deluxe, Capacity = 4 },
        }
    }
};

            _db.Hotels.AddRange(hotels);
            await _db.SaveChangesAsync();

            return Ok("Database seeded successfully.");
        }
    }
}
