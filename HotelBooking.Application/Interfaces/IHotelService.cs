using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

//documentation: This interface defines the contract for a hotel service, which is responsible for managing hotels in the application.
namespace HotelBooking.Application.Interfaces
{
    public interface IHotelService
    {
        Task<Hotel?> GetHotelByNameAsync(string name);
    }
}
