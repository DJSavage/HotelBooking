using Microsoft.AspNetCore.Mvc;
using HotelBooking.Application.Interfaces;
using HotelBooking.Api.Models;

namespace HotelBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/rooms/available?start=2025-01-01&end=2025-01-05&guests=2
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms(
            [FromQuery] DateOnly start,
            [FromQuery] DateOnly end,
            [FromQuery] int guests)
        {

            try
            {
                if (start >= end)
                    return BadRequest("Start date must be before end date.");

                if (guests <= 0)
                    return BadRequest("Guests must be greater than zero.");

                var rooms = await _roomService.GetAllRoomsAsync(start, end, guests);

                if (rooms.Count <= 0)
                    return BadRequest("No available rooms found for the specified criteria.");

                var result = rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Number = r.Number,
                    Capacity = r.Capacity,
                    RoomType = r.RoomType.ToString(),
                    HotelName = r.Hotel.Name
                }).ToList();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

            
        }
    }
}
