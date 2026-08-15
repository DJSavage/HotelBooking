using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;

//documentation: This class implements the IHotelService interface and provides methods for managing hotels in the application.
namespace HotelBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        //documentation: Constructor for the HotelService class, which takes an IHotelRepository as a parameter and initializes the _hotelRepository field.
        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        //documentation: This method retrieves a hotel by its name using the IHotelRepository.
        //It returns a Task that resolves to a Hotel object or null if the hotel is not found.
        public Task<Hotel?> GetHotelByNameAsync(string name)
            => _hotelRepository.GetHotelByNameAsync(name);
    }
}
