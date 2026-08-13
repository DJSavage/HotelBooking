using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBooking.Application.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllRoomsAsync(DateOnly start, DateOnly end, int guests);
    }
}
