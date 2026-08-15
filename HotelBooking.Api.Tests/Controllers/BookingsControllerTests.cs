using FluentAssertions;
using HotelBooking.Api.Controllers;
using HotelBooking.Api.Models;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

//documentation: This test class is designed to test the BookingsController in the HotelBooking.Api project. It uses Moq to mock dependencies and FluentAssertions for assertions.
namespace HotelBooking.Api.Tests.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Mock<IRoomService> _roomServiceMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;

        private readonly BookingsController _controller;

        //documentation: The constructor initializes the mocks and the controller instance for testing.
        // It sets up the necessary dependencies for the BookingsController.
        public BookingsControllerTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _roomServiceMock = new Mock<IRoomService>();
            _roomRepositoryMock = new Mock<IRoomRepository>();

            _controller = new BookingsController(
                _bookingServiceMock.Object,
                _roomServiceMock.Object,
                _roomRepositoryMock.Object
            );
        }

        //documentation: This test method checks the behavior of the CreateBooking action when the provided RoomId does not exist.
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenRoomIdDoesNotExist()
        {
            // Arrange
            var dto = new CreateBookingDto
            {
                RoomId = 999, // Assuming this room ID does not exist
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 12),
                Guests = 2
            };
            _roomServiceMock.Setup(service => service.GetRoomByIdAsync(dto.RoomId))
                            .ReturnsAsync((Room?)null); // Simulate room not found
            // Act
            var result = await _controller.CreateBooking(dto);
            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                  .Which.Value.Should().Be($"Room does not exist.");
        }

        //documentation: This test method checks the behavior of the CreateBooking action when the number of guests is zero.
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenGuestsAreZero()
        {
            // Arrange
            var dto = new CreateBookingDto
            {
                RoomId = 19,
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 12),
                Guests = 0
            };

            // Act
            var result = await _controller.CreateBooking(dto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Guests must be greater than zero.");
        }

        //documentation: This test method checks the behavior of the CreateBooking action when the start date is in the past.   
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenStartDateIsInThePast()
        {
            // Arrange
            var dto = new CreateBookingDto
            {
                RoomId = 19,
                StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), // Past date
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Guests = 2
            };
            // Act
            var result = await _controller.CreateBooking(dto);
            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Start date cannot be in the past.");
        }

        //documentation: This test method checks the behavior of the CreateBooking action when the start date is after the end date.
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenStartDateAfterEndDate()
        {
            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = new DateOnly(2028, 1, 15),
                EndDate = new DateOnly(2028, 1, 10),
                Guests = 2
            };

            var result = await _controller.CreateBooking(dto);

            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Start date must be before end date.");
        }

        //documentation: This test method checks the behavior of the CreateBooking action when the start date is equal to the end date.
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenStartDateEqualsEndDate()
        {
            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 10),
                Guests = 2
            };

            var result = await _controller.CreateBooking(dto);

            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Start and end dates cannot be the same.");
        }

        //documentation: This test method checks the behavior of the CreateBooking action when the room is unavailable for the selected dates.
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenRoomIsUnavailable()
        {
            var room = new Room { Id = 1, Capacity = 2 };

            var dto = new CreateBookingDto
            {
                RoomId = 1,
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 12),
                Guests = 2
            };

            _roomServiceMock.Setup(s => s.GetRoomByIdAsync(room.Id))
                            .ReturnsAsync(room);

            _bookingServiceMock.Setup(s =>
                s.CreateBookingAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<int>())
            ).ReturnsAsync((Booking?)null);

            var result = await _controller.CreateBooking(dto);

            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("Room is not available for the selected dates.");
        }

        //documentation: This test method checks the behavior of the CreateBooking action when a booking is successfully created.
        //It verifies that the response is a CreatedAtActionResult and that the returned BookingDto contains the expected values.
        [Fact]
        public async Task CreateBooking_ShouldReturnOk_WhenBookingIsCreated()
        {
            var room = new Room
            {
                Id = 2,
                Number = 101,
                RoomTypeId = 2,
                Capacity = 2,
                Hotel = new Hotel { Name = "Test Hotel" },
                RoomType = new RoomType
                {
                    Id = 2,
                    Name = "Double",
                    MaxGuests = 2,
                    BasePrice = 80
                }
            };

            var booking = new Booking
            {
                Id = 10,
                RoomId = 2,
                NumberOfGuests = 2,
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 12),
                Room = room
            };

            _roomServiceMock.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync(room);

            _roomRepositoryMock.Setup(r =>
                r.GetAvailableRoomsAsync(
                    It.IsAny<DateOnly>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<int>())
            ).ReturnsAsync(new List<Room> { room });

            _bookingServiceMock.Setup(s =>
                s.CreateBookingAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<int>())
            ).ReturnsAsync(booking);

            var dto = new CreateBookingDto
            {
                RoomId = 2,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Guests = 2
            };

            var result = await _controller.CreateBooking(dto);

            var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var returned = created.Value.Should().BeOfType<BookingDto>().Subject;

            returned.RoomId.Should().Be(2);
            returned.HotelName.Should().Be("Test Hotel");
            returned.RoomType.Should().Be("Double");
        }


        //documentation: This test method checks the behavior of the CreateBooking action when the provided RoomId is zero.
        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenRoomIdIsZero()
        {
            var dto = new CreateBookingDto
            {
                RoomId = 0,
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 12),
                Guests = 2
            };

            var result = await _controller.CreateBooking(dto);

            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("RoomId must be greater than zero.");
        }

    }
}