using FluentAssertions;
using HotelBooking.Api.Controllers;
using HotelBooking.Api.Models;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace HotelBooking.Api.Tests.Controllers;

public class RoomsControllerTests
{
    private readonly Mock<IRoomService> _roomServiceMock;
    private readonly RoomsController _controller;

    public RoomsControllerTests()
    {
        _roomServiceMock = new Mock<IRoomService>();
        _controller = new RoomsController(_roomServiceMock.Object);
    }

    // ------------------------------------------------------------
    // VALIDATION TESTS
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRooms_ShouldReturnBadRequest_WhenStartDateAfterEndDate()
    {
        var start = new DateOnly(2028, 1, 15);
        var end = new DateOnly(2028, 1, 10);

        var result = await _controller.GetAvailableRooms(start, end, 2);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("Start date must be before end date.");
    }

    [Fact]
    public async Task GetAvailableRooms_ShouldReturnBadRequest_WhenGuestsAreZero()
    {
        var start = new DateOnly(2028, 1, 10);
        var end = new DateOnly(2028, 1, 12);

        var result = await _controller.GetAvailableRooms(start, end, 0);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("Guests must be greater than zero.");
    }

    // ------------------------------------------------------------
    // EMPTY RESULTS TEST
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRooms_ShouldReturnBadRequest_WhenNoRoomsAvailable()
    {
        _roomServiceMock.Setup(s =>
            s.GetAllRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room>());

        var result = await _controller.GetAvailableRooms(
            new DateOnly(2028, 1, 10),
            new DateOnly(2028, 1, 12),
            2);

        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.Should().Be("No available rooms found for the specified criteria.");
    }

    // ------------------------------------------------------------
    // SUCCESSFUL RESULTS TEST
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRooms_ShouldReturnOk_WithMappedRoomDtos()
    {
        var room = new Room
        {
            Id = 1,
            Number = 101,
            Capacity = 2,
            RoomType = RoomType.Double,
            Hotel = new Hotel { Name = "Test Hotel" }
        };

        _roomServiceMock.Setup(s =>
            s.GetAllRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ReturnsAsync(new List<Room> { room });

        var result = await _controller.GetAvailableRooms(
            new DateOnly(2028, 1, 10),
            new DateOnly(2028, 1, 12),
            2);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var rooms = ok.Value.Should().BeAssignableTo<IEnumerable<RoomDto>>().Subject;

        rooms.Should().HaveCount(1);

        var dto = rooms.First();
        dto.Id.Should().Be(1);
        dto.Number.Should().Be(101);
        dto.Capacity.Should().Be(2);
        dto.RoomType.Should().Be("Double");
        dto.HotelName.Should().Be("Test Hotel");
    }

    // ------------------------------------------------------------
    // ERROR HANDLING TEST
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRooms_ShouldReturnInternalServerError_WhenServiceThrows()
    {
        _roomServiceMock.Setup(s =>
            s.GetAllRoomsAsync(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<int>())
        ).ThrowsAsync(new Exception("Database failure"));

        var result = await _controller.GetAvailableRooms(
            new DateOnly(2028, 1, 10),
            new DateOnly(2028, 1, 12),
            2);

        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(500);
    }
}
