using FinalProjectLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public class BookingRepository : IBookingRepository
    {
        private FinalProjectDBContext _context;
        public BookingRepository(FinalProjectDBContext context)
        {
            _context = context;
        }
        public async Task<bool> BookingExists(int bookingId)
        {
            return await _context.Bookings.AnyAsync<Booking>(r => r.BookingId == bookingId);
        }

        public async Task<bool> DeleteBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            _context.Bookings.Remove(booking);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> EditBooking(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookingExists(booking.BookingId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Booking> GetBookingById(int bookingId)
        {
            IQueryable<Booking> result = _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b=> b.BookingSlot)
                .ThenInclude(bs => bs.Restaurant)
                .Include(b => b.User);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookings()
        {
            var result = _context.Bookings
                .OrderBy(c => c.BookingId)
                .Include(b => b.BookingSlot)
                .ThenInclude(bs=>bs.Restaurant)
                .Include(b => b.User);
            return await result.ToListAsync();
        }

        public async Task<bool> SaveBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
