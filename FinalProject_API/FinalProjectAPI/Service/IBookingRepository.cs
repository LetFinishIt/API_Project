using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public interface IBookingRepository
    {
        Task<bool> BookingExists(int bookingId);
        Task<IEnumerable<Booking>> GetBookings();
        Task<Booking> GetBookingById(int bookingId);
        Task<bool> SaveBooking(Booking booking);
        Task<bool> EditBooking(Booking booking);
        Task<bool> DeleteBooking(int bookingId);
    }
}
