using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public interface IBookingSlotRepository
    {
        Task<bool> BookingSlotExists(int bookingSlotId);
        Task<IEnumerable<BookingSlot>> GetBookingSlots();
        Task<BookingSlot> GetBookingSlotById(int bookingSlotId);
        Task<bool> SaveBookingSlot(BookingSlot bookingSlot);
        Task<bool> EditBookingSlot(BookingSlot bookingSlot);
        Task<bool> DeleteBookingSlot(int bookingSlotId);
    }
}
