using FluentAssertions;
using HotelBooking.Api.Controllers;
using HotelBooking.Api.Models;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

//docmentation: This test class is designed to test the HotelsController in the HotelBooking API.
//It uses Moq to mock the IHotelService dependency and FluentAssertions for expressive assertions.
//The tests cover various scenarios, including when a hotel is not found, when a hotel is found with rooms, when a hotel is found with no rooms, and when the service throws an exception.
namespace HotelBooking.Api.Tests.Controllers;

public class HotelsControllerTests
{
    private readonly Mock<IHotelService> _hotelServiceMock;
    private readonly HotelsController _controller;

    public HotelsControllerTests()
    {
        _hotelServiceMock = new Mock<IHotelService>();
        _controller = new HotelsController(_hotelServiceMock.Object);
    }

    // ------------------------------------------------------------
    // HOTEL NOT FOUND
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByName_ShouldReturnNotFound_WhenHotelDoesNotExist()
    {
        _hotelServiceMock.Setup(s => s.GetHotelByNameAsync("UnknownHotel"))
                         .ReturnsAsync((Hotel?)null);

        var result = await _controller.GetHotelByName("UnknownHotel");

        result.Should().BeOfType<NotFoundObjectResult>()
              .Which.Value.Should().Be("Hotel 'UnknownHotel' not found.");
    }

    // ------------------------------------------------------------
    // HOTEL FOUND → OK
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByName_ShouldReturnOk_WithMappedHotelDto()
    {
        var hotel = new Hotel
        {
            Id = 1,
            Name = "GrandHotel",
            Rooms = new List<Room>
            {
                new Room
                {
                    Id = 10,
                    Number = 101,
                    Capacity = 2,
                    RoomTypeId = 2,
                    RoomType = new RoomType
                    {
                        Id = 2,
                        Name = "Double",
                        MaxGuests = 2,
                        BasePrice = 80
                    }
                }
            }
        };

        _hotelServiceMock.Setup(s => s.GetHotelByNameAsync("GrandHotel"))
                         .ReturnsAsync(hotel);

        var result = await _controller.GetHotelByName("GrandHotel");

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = ok.Value.Should().BeOfType<HotelDto>().Subject;

        dto.Id.Should().Be(1);
        dto.Name.Should().Be("GrandHotel");
        dto.Rooms.Should().HaveCount(1);

        var roomDto = dto.Rooms.First();
        roomDto.Id.Should().Be(10);
        roomDto.Number.Should().Be(101);
        roomDto.Capacity.Should().Be(2);
        roomDto.RoomType.Should().Be("Double");
    }

    // ------------------------------------------------------------
    // HOTEL FOUND WITH NO ROOMS
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByName_ShouldReturnOk_WhenHotelHasNoRooms()
    {
        var hotel = new Hotel
        {
            Id = 2,
            Name = "EmptyHotel",
            Rooms = new List<Room>()
        };

        _hotelServiceMock.Setup(s => s.GetHotelByNameAsync("EmptyHotel"))
                         .ReturnsAsync(hotel);

        var result = await _controller.GetHotelByName("EmptyHotel");

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = ok.Value.Should().BeOfType<HotelDto>().Subject;

        dto.Id.Should().Be(2);
        dto.Name.Should().Be("EmptyHotel");
        dto.Rooms.Should().BeEmpty();
    }

    // ------------------------------------------------------------
    // SERVICE THROWS → INTERNAL SERVER ERROR
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByName_ShouldReturnInternalServerError_WhenServiceThrows()
    {
        _hotelServiceMock.Setup(s => s.GetHotelByNameAsync(It.IsAny<string>()))
                         .ThrowsAsync(new Exception("Database failure"));

        var result = await _controller.GetHotelByName("GrandHotel");

        var error = result.Should().BeOfType<ObjectResult>().Subject;
        error.StatusCode.Should().Be(500);
    }
}
