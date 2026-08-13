using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();

            return services;
        }
    }
}
