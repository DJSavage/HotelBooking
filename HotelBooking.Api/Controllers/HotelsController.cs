using Microsoft.AspNetCore.Mvc;
using HotelBooking.Application.Interfaces;
using HotelBooking.Api.Models;

//documentation: This controller provides endpoints for managing hotels, including retrieving hotel details by name.
//It interacts with the IHotelService to fetch hotel data and returns it in a structured format.
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
        // Retrieves hotel details by hotel name.
        [HttpGet("{name}")]
        public async Task<IActionResult> GetHotelByName(string name)
        {
            try
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
                        RoomType = r.RoomType.Name,
                        RoomTypeId = r.RoomTypeId
                    }).ToList()
                };

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
            
        }
    }
}
