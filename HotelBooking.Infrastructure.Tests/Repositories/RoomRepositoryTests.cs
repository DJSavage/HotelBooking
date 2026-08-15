using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using System.Collections.Generic;

namespace HotelBooking.Infrastructure.Tests.Repositories;

public class RoomRepositoryTests
{
    private BookingDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BookingDbContext(options);
    }

    // ------------------------------------------------------------
    // GET ROOM BY ID
    // ------------------------------------------------------------

    [Fact]
    public async Task GetRoomByIdAsync_ShouldReturnRoom_WhenExists()
    {
        using var db = CreateDbContext();

        var room = new Room { Id = 1, Number = 101, Capacity = 2 };
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        var repo = new RoomRepository(db);

        var result = await repo.GetRoomByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetRoomByIdAsync_ShouldIncludeBookings()
    {
        using var db = CreateDbContext();

        var room = new Room { Id = 1 };
        room.Bookings.Add(new Booking
        {
            RoomId = 1,
            StartDate = new DateOnly(2028, 1, 10),
            EndDate = new DateOnly(2028, 1, 12),
            BookingReference = Guid.NewGuid().ToString()
        });

        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        var repo = new RoomRepository(db);

        var result = await repo.GetRoomByIdAsync(1);

        result!.Bookings.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetRoomByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        using var db = CreateDbContext();
        var repo = new RoomRepository(db);

        var result = await repo.GetRoomByIdAsync(999);

        result.Should().BeNull();
    }
    

    // ------------------------------------------------------------
    // GET AVAILABLE ROOMS — OVERLAPPING BOOKINGS
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRoomsAsync_ShouldExcludeRoomsWithOverlappingBookings()
    {
        // Arrange: create in-memory EF context
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new BookingDbContext(options);

        // Seed a room with an overlapping booking
        var room = new Room
        {
            Id = 1,
            Capacity = 2,
            Bookings = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                RoomId = 1,
                StartDate = new DateOnly(2028, 1, 10),
                EndDate = new DateOnly(2028, 1, 12),
                BookingReference = Guid.NewGuid().ToString()
            }
        }
        };

        context.Rooms.Add(room);
        context.SaveChanges();

        var repository = new RoomRepository(context);

        // Act: request dates that OVERLAP the existing booking
        var result = await repository.GetAvailableRoomsAsync(
            new DateOnly(2028, 1, 11),   // Overlaps
            new DateOnly(2028, 1, 13),   // Overlaps
            2
        );

        // Assert: room should be excluded
        result.Should().HaveCount(0);
    }
    

    // ------------------------------------------------------------
    // GET AVAILABLE ROOMS — HOTEL INCLUDED
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRoomsAsync_ShouldIncludeHotelNavigationProperty()
    {
        using var db = CreateDbContext();

        var hotel = new Hotel { Id = 1, Name = "Test Hotel" };

        var room = new Room
        {
            Id = 1,
            Capacity = 2,
            Hotel = hotel
        };

        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        var repo = new RoomRepository(db);

        var result = await repo.GetAvailableRoomsAsync(
            new DateOnly(2028, 1, 10),
            new DateOnly(2028, 1, 12),
            guests: 2);

        result.Should().HaveCount(1);
        result.First().Hotel.Should().NotBeNull();
        result.First().Hotel!.Name.Should().Be("Test Hotel");
    }

    // ------------------------------------------------------------
    // GET AVAILABLE ROOMS — NO MATCH
    // ------------------------------------------------------------

    [Fact]
    public async Task GetAvailableRoomsAsync_ShouldReturnEmptyList_WhenNoRoomsMatch()
    {
        using var db = CreateDbContext();

        db.Rooms.Add(new Room { Id = 1, Capacity = 1 });
        await db.SaveChangesAsync();

        var repo = new RoomRepository(db);

        var result = await repo.GetAvailableRoomsAsync(
            new DateOnly(2028, 1, 10),
            new DateOnly(2028, 1, 12),
            guests: 3);

        result.Should().BeEmpty();
    }
}
