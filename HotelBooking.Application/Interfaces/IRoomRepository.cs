using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces
{
    //documentation: This interface defines the contract for a room repository, which is responsible for managing hotel rooms in the application.
    public interface IRoomRepository
    {
        Task<List<Room>> GetAvailableRoomsAsync(DateOnly start, DateOnly end, int guests);

        Task<Room?> GetRoomByIdAsync(int roomId);
    }
}
