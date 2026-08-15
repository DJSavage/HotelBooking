using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public Task<Booking?> GetByReferenceAsync(string reference)
        => _bookingRepository.GetByReferenceAsync(reference);

        public async Task<Booking?> CreateBookingAsync(int roomId, DateOnly start, DateOnly end, int guests)
        {
            // Check availability
            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(start, end, guests);
            var room = availableRooms.FirstOrDefault(r => r.Id == roomId);

            if (room is null)
                return null;

            var booking = new Booking
            {
                RoomId = roomId,
                StartDate = start,
                EndDate = end,
                NumberOfGuests = guests,
                BookingReference = GenerateReference(roomId, start)
            };

            await _bookingRepository.CreateBookingAsync(booking);

            return booking;
        }

        private string GenerateReference(int roomId, DateOnly startDate)
        {
            // Date part: 20260813 → YYMMDD
            var datePart = startDate.ToString("yyMMdd");

            // Room part: zero‑padded (e.g., 005)
            var roomPart = roomId.ToString("D3");

            // Random part: 4 uppercase characters
            var randomPart = Guid.NewGuid().ToString("N")[..4].ToUpper();

            return $"BK-{datePart}-{roomPart}-{randomPart}";
        }
    }
}
