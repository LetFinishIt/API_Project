using FinalProjectLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public class BookingSlotRepository : IBookingSlotRepository
    {
        private FinalProjectDBContext _context;
        public BookingSlotRepository(FinalProjectDBContext context)
        {
            _context = context;
        }

        public async Task<bool> BookingSlotExists(int bookingSlotId)
        {
            return await _context.BookingSlots.AnyAsync<BookingSlot>(bs => bs.BookingSlotId == bookingSlotId);
        }

        public async Task<bool> DeleteBookingSlot(int bookingSlotId)
        {
            var bookingSlot = await _context.BookingSlots.FindAsync(bookingSlotId);
            if (bookingSlot == null)
            {
                return false;
            }

            _context.BookingSlots.Remove(bookingSlot);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> EditBookingSlot(BookingSlot bookingSlot)
        {
            _context.Entry(bookingSlot).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookingSlotExists(bookingSlot.BookingSlotId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<BookingSlot> GetBookingSlotById(int bookingSlotId)
        {
            IQueryable<BookingSlot> result = _context.BookingSlots
                .Where(bs => bs.BookingSlotId == bookingSlotId)
                .Include(bs=>bs.Bookings)
                .ThenInclude(b => b.User)
                .Include(bs => bs.Restaurant);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BookingSlot>> GetBookingSlots()
        {
            var result = _context.BookingSlots
                .OrderBy(bs => bs.BookingSlotId)
                .Include(bs=>bs.Bookings)
                .ThenInclude(b=>b.User)
                .Include(bs=>bs.Restaurant);

            return await result.ToListAsync();
        }

        public async Task<bool> SaveBookingSlot(BookingSlot bookingSlot)
        {
            _context.BookingSlots.Add(bookingSlot);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
