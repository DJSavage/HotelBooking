using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public Task<Hotel?> GetHotelByNameAsync(string name)
            => _hotelRepository.GetHotelByNameAsync(name);
    }
}
