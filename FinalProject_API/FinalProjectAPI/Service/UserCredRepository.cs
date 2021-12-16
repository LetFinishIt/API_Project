using FinalProjectLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public class UserCredRepository : IUserCredRepository
    {
        private FinalProjectDBContext _context;

        public UserCredRepository(FinalProjectDBContext context)
        {
            _context = context;
        }

        public async Task<UserCred> GetUserByEmailPassword(string email, string password)
        {
            IQueryable<UserCred> result = _context.UserCreds.Where(uc => uc.Email == email && uc.Password==password);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteUserCred(int userId)
        {
            var user = await _context.UserCreds.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _context.UserCreds.Remove(user);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> EditUserCred(UserCred user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserCredExists(user.UserId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<UserCred> GetUserCredById(int userId)
        {
            IQueryable<UserCred> result = _context.UserCreds.Where(uc => uc.UserId == userId)
                .Include(uc => uc.Restaurants)
                .Include(uc=>uc.Bookings)
                .ThenInclude(b => b.BookingSlot);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserCred>> GetUserCreds()
        {
            var result = _context.UserCreds.OrderBy(uc => uc.UserId)
                .Include(uc => uc.Restaurants)
                .Include(uc=>uc.Bookings)
                .ThenInclude(b => b.BookingSlot);
            return await result.ToListAsync();
        }

        public async Task<bool> SaveUserCred(UserCred user)
        {
            _context.UserCreds.Add(user);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UserCredExists(int userId)
        {
            return await _context.UserCreds.AnyAsync<UserCred>(uc => uc.UserId == userId);
        }
    }
}
