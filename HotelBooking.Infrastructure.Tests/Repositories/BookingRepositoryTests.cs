using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using System.Collections.Generic;

namespace HotelBooking.Infrastructure.Tests.Repositories;

public class BookingRepositoryTests
{
    private BookingDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BookingDbContext(options);
    }

    // ------------------------------------------------------------
    // GET BY REFERENCE — FOUND
    // ------------------------------------------------------------

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnBooking_WhenExists()
    {
        using var db = CreateDbContext();

        var hotel = new Hotel { Id = 1, Name = "Test Hotel" };
        var room = new Room { Id = 10, Number = 101, Hotel = hotel };

        var booking = new Booking
        {
            Id = 100,
            BookingReference = "BK-ABC123",
            Room = room
        };

        db.Bookings.Add(booking);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var repo = new BookingRepository(db);

        var result = await repo.GetByReferenceAsync("BK-ABC123");

        result.Should().NotBeNull();
        result!.BookingReference.Should().Be("BK-ABC123");
        result.Room.Should().NotBeNull();
        result.Room!.Hotel!.Name.Should().Be("Test Hotel");
    }

    // ------------------------------------------------------------
    // GET BY REFERENCE — NOT FOUND
    // ------------------------------------------------------------

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnNull_WhenNotFound()
    {
        using var db = CreateDbContext();
        var repo = new BookingRepository(db);

        var result = await repo.GetByReferenceAsync("UNKNOWN");

        result.Should().BeNull();
    }

    // ------------------------------------------------------------
    // GET BY REFERENCE — CASE SENSITIVE
    // ------------------------------------------------------------

    [Fact]
    public async Task GetByReferenceAsync_ShouldBeCaseSensitive()
    {
        using var db = CreateDbContext();

        db.Bookings.Add(new Booking
        {
            Id = 1,
            BookingReference = "BK-ABC123"
        });

        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var repo = new BookingRepository(db);

        var result = await repo.GetByReferenceAsync("bk-abc123"); // lowercase

        result.Should().BeNull();
    }

    // ------------------------------------------------------------
    // CREATE BOOKING — SAVES AND RETURNS HYDRATED BOOKING
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldSaveBooking_AndReturnHydratedBooking()
    {
        using var db = CreateDbContext();

        var hotel = new Hotel { Id = 1, Name = "Test Hotel" };
        var room = new Room { Id = 10, Number = 101, Hotel = hotel };

        db.Rooms.Add(room);
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var booking = new Booking
        {
            BookingReference = "BK-XYZ999",
            RoomId = 10
        };

        var repo = new BookingRepository(db);

        var result = await repo.CreateBookingAsync(booking);

        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        result.BookingReference.Should().Be("BK-XYZ999");

        // Navigation properties must be loaded
        result.Room.Should().NotBeNull();
        result.Room!.Number.Should().Be(101);
        result.Room!.Hotel!.Name.Should().Be("Test Hotel");
    }

    // ------------------------------------------------------------
    // CREATE BOOKING — MULTIPLE BOOKINGS
    // ------------------------------------------------------------

    [Fact]
    public async Task CreateBookingAsync_ShouldReturnCorrectBooking_WhenMultipleExist()
    {
        using var db = CreateDbContext();

        var hotel = new Hotel { Id = 1, Name = "Test Hotel" };
        var room = new Room { Id = 10, Number = 101, Hotel = hotel };

        db.Rooms.Add(room);

        db.Bookings.Add(new Booking
        {
            Id = 1,
            BookingReference = "BK-OLD"
        });

        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var newBooking = new Booking
        {
            BookingReference = "BK-NEW",
            RoomId = 10
        };

        var repo = new BookingRepository(db);

        var result = await repo.CreateBookingAsync(newBooking);

        result.Should().NotBeNull();
        result!.BookingReference.Should().Be("BK-NEW");
        result.Room!.Hotel!.Name.Should().Be("Test Hotel");
    }
}
