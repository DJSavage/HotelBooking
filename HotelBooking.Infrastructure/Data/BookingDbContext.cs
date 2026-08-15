using Microsoft.EntityFrameworkCore;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Infrastructure.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options) { }

    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomType>()
    .Property(rt => rt.BasePrice)
    .HasPrecision(10, 2);

        modelBuilder.Entity<RoomType>().HasData(
    new RoomType { Id = 1, Name = "Single", MaxGuests = 1, BasePrice = 50 },
    new RoomType { Id = 2, Name = "Double", MaxGuests = 2, BasePrice = 80 },
    new RoomType { Id = 3, Name = "Deluxe", MaxGuests = 4, BasePrice = 150 }
);

        modelBuilder.Entity<Booking>()
            .HasIndex(b => b.BookingReference)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
