using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces
{
    //documentation: This interface defines the contract for a hotel repository, which is responsible for managing hotels in the application.
    public interface IHotelRepository
    {
        Task<Hotel?> GetHotelByNameAsync(string name);
    }
}
