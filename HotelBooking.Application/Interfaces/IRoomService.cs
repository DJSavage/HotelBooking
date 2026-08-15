using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

//documentation: This interface defines the contract for a room service, which is responsible for managing hotel rooms in the application.
namespace HotelBooking.Application.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllRoomsAsync(DateOnly start, DateOnly end, int guests);

        Task<Room?> GetRoomByIdAsync(int roomId);
    }
}
