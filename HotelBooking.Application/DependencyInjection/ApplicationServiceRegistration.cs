using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

//documentation: This static class provides an extension method to register application services with the dependency injection container.
namespace HotelBooking.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        //documentation: This method registers application services with the dependency injection container.
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();

            return services;
        }
    }
}
