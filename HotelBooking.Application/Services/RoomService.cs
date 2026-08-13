using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public Task<List<Room>> GetAllRoomsAsync(DateOnly start, DateOnly end, int guests)
        => _roomRepository.GetAvailableRoomsAsync(start, end, guests);
        
    }
}
