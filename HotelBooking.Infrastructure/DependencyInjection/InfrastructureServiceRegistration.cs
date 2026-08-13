using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Application.Interfaces;
using HotelBooking.Infrastructure.Repositories;

//documentation: this class is used to register the infrastructure services in the DI container. It is called from the Program.cs file in the API project.

namespace HotelBooking.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        // <summary>
        // Registers the infrastructure services in the DI container.
        // </summary>
        // <param name="services">The service collection.</param>
        // <param name="config">The configuration.</param>
        // <returns>The updated service collection.</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            //add dbcontext
            services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            //Register infrastructure services
            //use addscoped so that one instance of the repository is created per request

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            return services;
        }
    }
}
