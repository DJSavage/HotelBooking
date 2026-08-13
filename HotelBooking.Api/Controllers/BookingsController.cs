using Microsoft.AspNetCore.Mvc;
using HotelBooking.Application.Interfaces;
using HotelBooking.Api.Models;

namespace HotelBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
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
            if (dto.StartDate >= dto.EndDate)
                return BadRequest("Start date must be before end date.");

            if (dto.Guests <= 0)
                return BadRequest("Guests must be greater than zero.");

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
