using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetHotelByNameAsync(string name);
    }
}
