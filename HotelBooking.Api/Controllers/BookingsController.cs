using HotelBooking.Api.Models;
using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IRoomRepository _roomRepository;

        public BookingsController(IBookingService bookingService, IRoomService roomService, IRoomRepository roomRepository)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _roomRepository = roomRepository;
        }

        // GET: api/bookings/{reference}
        [HttpGet("{reference}")]
        public async Task<IActionResult> GetBookingByReference(string reference)
        {
            var booking = await _bookingService.GetByReferenceAsync(reference);

            if (booking is null)
                return NotFound($"Booking '{reference}' not found.");

            var result = new BookingDto
            {
                BookingReference = booking.BookingReference,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Guests = booking.NumberOfGuests,
                RoomId = booking.RoomId,
                HotelName = booking.Room.Hotel.Name,
                RoomType = booking.Room.RoomType.ToString()
            };

            return Ok(result);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            if (dto.StartDate < DateOnly.FromDateTime(DateTime.Today))
                return BadRequest("Start date cannot be in the past.");

            if (dto.StartDate == dto.EndDate)
                return BadRequest("Start and end dates cannot be the same.");

            if (dto.StartDate >= dto.EndDate)
                return BadRequest("Start date must be before end date.");

            if (dto.Guests <= 0)
                return BadRequest("Guests must be greater than zero.");

            if(dto.RoomId <= 0)
                return BadRequest("RoomId must be greater than zero.");

            var room = await _roomService.GetRoomByIdAsync(dto.RoomId);
            if (room is null)
                return NotFound("Room does not exist.");

            if (dto.Guests > room.Capacity)
                return BadRequest("Guest count exceeds room capacity.");

            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(dto.StartDate, dto.EndDate, dto.Guests);

            if (availableRooms == null || !availableRooms.Any(r => r.Id == dto.RoomId))
                return BadRequest("Room is not available for the selected dates.");

            var booking = await _bookingService.CreateBookingAsync(
                dto.RoomId,
                dto.StartDate,
                dto.EndDate,
                dto.Guests
            );

            if (booking is null)
                return BadRequest("Room is not available for the selected dates.");

            var result = new BookingDto
            {
                BookingReference = booking.BookingReference,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Guests = booking.NumberOfGuests,
                RoomId = booking.RoomId,
                HotelName = booking.Room.Hotel.Name,
                RoomType = booking.Room.RoomType.ToString()
            };

            return CreatedAtAction(nameof(GetBookingByReference),
                new { reference = booking.BookingReference },
                result);
        }

    }
}
