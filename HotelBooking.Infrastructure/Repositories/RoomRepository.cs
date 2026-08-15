using HotelBooking.Application.Interfaces;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

//documentation: This class implements the IRoomRepository interface and provides methods for accessing room data from the database.
//It uses Entity Framework Core to interact with the database and retrieve available rooms based on the specified criteria.
// The GetAvailableRoomsAsync method retrieves a list of available rooms that meet the specified criteria, including the start and end dates and the number of guests.
// It filters the rooms based on their capacity and checks for any overlapping bookings to ensure that only available rooms are returned.
// The RoomRepository class is part of the HotelBooking.Infrastructure.Repositories namespace and is responsible for managing room data in the application.
// It provides a way to retrieve available rooms from the database based on specific criteria, making it easier to manage room availability and bookings in the hotel booking system.
// The class is designed to be used in conjunction with other components of the application, such as services and controllers, to provide a complete solution for managing hotel bookings and room availability.
// The RoomRepository class is a key component of the hotel booking system, providing a way to access and manage room data in the database.
// It is designed to be flexible and extensible, allowing for future enhancements and modifications as needed.
// The class is also designed to be testable, allowing for unit tests to be written to verify its functionality and ensure that it behaves as expected in different scenarios.
// The RoomRepository class is an important part of the overall architecture of the hotel booking system, providing a way to manage room data and availability in a consistent and efficient manner. It is designed to be easy to use and understand, making it accessible to developers of all skill levels.
namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BookingDbContext _db;

        public RoomRepository(BookingDbContext db)
        {
            _db = db;
        }

        //documentation: This method retrieves a list of available rooms that meet the specified criteria, including the start and end dates and the number of guests.
        //It filters the rooms based on their capacity and checks for any overlapping bookings to ensure that only available rooms are returned.
        // The method uses Entity Framework Core to query the database and retrieve the relevant data.
        // It includes the Bookings navigation property to check for any existing bookings that may conflict with the specified dates.
        // The method returns a list of Room objects that meet the specified criteria, allowing for easy access to the available rooms in the application.
        // The method is designed to be asynchronous, allowing for efficient and responsive database access in the application.
        // It uses the ToListAsync method to retrieve the results from the database and return them as a list of Room objects.
        // The method is part of the RoomRepository class, which is responsible for managing room data in the application.
        // It provides a way to retrieve available rooms from the database based on specific criteria, making it easier to manage room availability and bookings in the hotel booking system.

        public async Task<List<Room>> GetAvailableRoomsAsync(DateOnly start, DateOnly end, int guests)
        {
            return await _db.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Bookings)
                .Where(r => r.Capacity >= guests)
                .Where(r => r.Bookings.All(b =>
                    end <= b.StartDate || start >= b.EndDate))
                .ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int roomId)
        {
            return await _db.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

    }
}