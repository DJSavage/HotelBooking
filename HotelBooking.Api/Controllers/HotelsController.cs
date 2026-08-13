using Microsoft.AspNetCore.Mvc;
using HotelBooking.Application.Interfaces;
using HotelBooking.Api.Models;

namespace HotelBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // GET: api/hotels/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetHotelByName(string name)
        {
            var hotel = await _hotelService.GetHotelByNameAsync(name);

            if (hotel is null)
                return NotFound($"Hotel '{name}' not found.");

            var result = new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rooms = hotel.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Number = r.Number,
                    Capacity = r.Capacity,
                    RoomType = r.RoomType.ToString()
                }).ToList()
            };

            return Ok(result);
        }
    }
}
