using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAvailableRoomsAsync(DateOnly start, DateOnly end, int guests);
    }
}
