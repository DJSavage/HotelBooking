using FluentAssertions;
using Moq;
using Xunit;
using HotelBooking.Application.Services;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using System.Collections.Generic;

//documentation: This test class is designed to test the BookingService class, which is responsible for managing hotel bookings.
//It uses Moq to create mock implementations of the IBookingRepository and IRoomRepository interfaces, allowing for isolated testing of the BookingService's behavior without relying on actual database operations.
//The tests cover various scenarios, including retrieving bookings by reference, creating bookings under different conditions, and ensuring that the correct booking data is passed to the repository.

namespace HotelBooking.Application.Tests.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly Mock<IRoomRepository> _roomRepoMock;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _roomRepoMock = new Mock<IRoomRepository>();

        _service = new BookingService(_bookingRepoMock.Object, _roomRepoMock.Object);
    }

    // ------------------------------------------------------------
    // GET BY REFERENCE
    // ------------------------------------------------------------

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnBooking_WhenFound()
    {
        var booking = new Booking { BookingReference = "BK-TEST" };

        _bookingRepoMock.Setup(r => r.GetByReferenceAsync("BK-TEST"))
                        .ReturnsAsync(booking);

        var result = await _service.GetByReferenceAsync("BK-TEST");

        result.Should().NotBeNull();
        result!.BookingReference.Should().Be("BK-TEST");
    }

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnNull_WhenNotFound()
    {
        _bookingRepoMock.Setup(r => r.GetByReferenceAsync("UNKNOWN"))
                        .ReturnsAsync((Booking?)null);

        var result = await _service.GetByReferenceAsync("UNKNOWN");

        result.Should().BeNull();
    }

    // ------------------------------------------------------------
    // CREATE BOOKING — ROOM UNAVAILABLE
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldReturnNull_WhenRoomIsNotAvailable()
    {
        _roomRepoMock.Setup(r =>
            r.GetAvailableRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room>()); // no rooms

        var result = await _service.CreateBookingAsync(1, new DateOnly(2028, 1, 10), new DateOnly(2028, 1, 12), 2);

        result.Should().BeNull();
        _bookingRepoMock.Verify(r => r.CreateBookingAsync(It.IsAny<Booking>()), Times.Never);
    }

    // ------------------------------------------------------------
    // CREATE BOOKING — ROOM EXISTS BUT NOT MATCHING roomId
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldReturnNull_WhenRoomIdNotInAvailableRooms()
    {
        var availableRooms = new List<Room>
        {
            new Room { Id = 2, Capacity = 2 }
        };

        _roomRepoMock.Setup(r =>
            r.GetAvailableRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(availableRooms);

        var result = await _service.CreateBookingAsync(1, new DateOnly(2028, 1, 10), new DateOnly(2028, 1, 12), 2);

        result.Should().BeNull();
        _bookingRepoMock.Verify(r => r.CreateBookingAsync(It.IsAny<Booking>()), Times.Never);
    }

    // ------------------------------------------------------------
    // SUCCESSFUL BOOKING
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldCreateBooking_WhenRoomIsAvailable()
    {
        var room = new Room { Id = 1, Capacity = 2 };

        _roomRepoMock.Setup(r =>
            r.GetAvailableRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room> { room });

        _bookingRepoMock.Setup(r =>
            r.CreateBookingAsync(It.IsAny<Booking>())
        ).ReturnsAsync((Booking b) => b);

        var start = new DateOnly(2028, 1, 10);
        var end = new DateOnly(2028, 1, 12);

        var result = await _service.CreateBookingAsync(1, start, end, 2);

        result.Should().NotBeNull();
        result!.RoomId.Should().Be(1);
        result.StartDate.Should().Be(start);
        result.EndDate.Should().Be(end);
        result.NumberOfGuests.Should().Be(2);
        result.BookingReference.Should().StartWith("BK-");
    }

    // ------------------------------------------------------------
    // BOOKING REFERENCE FORMAT
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldGenerateCorrectReferenceFormat()
    {
        var room = new Room { Id = 5, Capacity = 2 };

        _roomRepoMock.Setup(r =>
            r.GetAvailableRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room> { room });

        _bookingRepoMock.Setup(r =>
            r.CreateBookingAsync(It.IsAny<Booking>())
        ).ReturnsAsync((Booking b) => b);

        var start = new DateOnly(2028, 8, 13); // 260813

        var result = await _service.CreateBookingAsync(5, start, new DateOnly(2028, 8, 15), 2);

        result.Should().NotBeNull();
        result!.BookingReference.Should().MatchRegex(@"^BK-\d{6}-\d{3}-[A-Z0-9]{4}$");
    }

    // ------------------------------------------------------------
    // REPOSITORY FAILURE
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldThrow_WhenRepositoryThrows()
    {
        var room = new Room { Id = 1, Capacity = 2 };

        _roomRepoMock.Setup(r =>
            r.GetAvailableRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room> { room });

        _bookingRepoMock.Setup(r =>
            r.CreateBookingAsync(It.IsAny<Booking>())
        ).ThrowsAsync(new Exception("DB failure"));

        Func<Task> act = async () =>
            await _service.CreateBookingAsync(1, new DateOnly(2028, 1, 10), new DateOnly(2028, 1, 12), 2);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("DB failure");
    }

    // ------------------------------------------------------------
    // ENSURE CORRECT BOOKING OBJECT IS PASSED TO REPOSITORY
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldPassCorrectBookingToRepository()
    {
        var room = new Room { Id = 1, Capacity = 2 };

        _roomRepoMock.Setup(r =>
            r.GetAvailableRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room> { room });

        Booking? captured = null;

        _bookingRepoMock.Setup(r =>
            r.CreateBookingAsync(It.IsAny<Booking>())
        ).Callback<Booking>(b => captured = b)
         .ReturnsAsync((Booking b) => b);

        var start = new DateOnly(2028, 1, 10);
        var end = new DateOnly(2028, 1, 12);

        await _service.CreateBookingAsync(1, start, end, 2);

        captured.Should().NotBeNull();
        captured!.RoomId.Should().Be(1);
        captured.StartDate.Should().Be(start);
        captured.EndDate.Should().Be(end);
        captured.NumberOfGuests.Should().Be(2);
        captured.BookingReference.Should().StartWith("BK-");
    }
}
