using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelService
    {
        Task<Hotel?> GetHotelByNameAsync(string name);
    }
}
