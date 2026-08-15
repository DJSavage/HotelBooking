using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using System.Collections.Generic;

//documentation: This test class is designed to test the HotelRepository class, which is responsible for managing hotels in the database.
namespace HotelBooking.Infrastructure.Tests.Repositories;

public class HotelRepositoryTests
{
    private BookingDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BookingDbContext(options);
    }

    // ------------------------------------------------------------
    // HOTEL FOUND
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByNameAsync_ShouldReturnHotel_WhenExists()
    {
        using var db = CreateDbContext();

        var hotel = new Hotel
        {
            Id = 1,
            Name = "GrandHotel"
        };

        db.Hotels.Add(hotel);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var repo = new HotelRepository(db);

        var result = await repo.GetHotelByNameAsync("GrandHotel");

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("GrandHotel");
    }

    // ------------------------------------------------------------
    // HOTEL NOT FOUND
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByNameAsync_ShouldReturnNull_WhenNotFound()
    {
        using var db = CreateDbContext();
        var repo = new HotelRepository(db);

        var result = await repo.GetHotelByNameAsync("UnknownHotel");

        result.Should().BeNull();
    }

    // ------------------------------------------------------------
    // ROOMS INCLUDED
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByNameAsync_ShouldIncludeRooms()
    {
        using var db = CreateDbContext();

        var hotel = new Hotel
        {
            Id = 1,
            Name = "GrandHotel",
            Rooms = new List<Room>
            {
                new Room { Id = 10, Number = 101, Capacity = 2 },
                new Room { Id = 11, Number = 102, Capacity = 3 }
            }
        };

        db.Hotels.Add(hotel);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var repo = new HotelRepository(db);

        var result = await repo.GetHotelByNameAsync("GrandHotel");

        result.Should().NotBeNull();
        result!.Rooms.Should().HaveCount(2);
        result.Rooms.First().Number.Should().Be(101);
    }


    // ------------------------------------------------------------
    // MULTIPLE HOTELS — RETURNS CORRECT ONE
    // ------------------------------------------------------------

    [Fact]
    public async Task GetHotelByNameAsync_ShouldReturnCorrectHotel_WhenMultipleExist()
    {
        using var db = CreateDbContext();

        db.Hotels.AddRange(
            new Hotel { Id = 1, Name = "HotelA" },
            new Hotel { Id = 2, Name = "HotelB" }
        );

        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var repo = new HotelRepository(db);

        var result = await repo.GetHotelByNameAsync("HotelB");

        result.Should().NotBeNull();
        result!.Id.Should().Be(2);
        result.Name.Should().Be("HotelB");
    }
}
