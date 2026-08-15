using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;

//documentation: This class implements the IRoomService interface and provides methods for managing hotel rooms in the application.
namespace HotelBooking.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        //documentation: This method retrieves a list of available rooms based on the specified start and end dates, as well as the number of guests.
        public Task<List<Room>> GetAllRoomsAsync(DateOnly start, DateOnly end, int guests)
        => _roomRepository.GetAvailableRoomsAsync(start, end, guests);

        //documentation: This method retrieves a room by its ID using the IRoomRepository.
        public Task<Room?> GetRoomByIdAsync(int roomId)
            => _roomRepository.GetRoomByIdAsync(roomId);


    }
}
