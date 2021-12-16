using FinalProjectLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private FinalProjectDBContext _context; 
        public RestaurantRepository(FinalProjectDBContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteRestaurant(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return false;
            }

            _context.Restaurants.Remove(restaurant);
            return (await _context.SaveChangesAsync())>0;
        }

        public async Task<bool> EditRestaurant(Restaurant restaurant)
        {
            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
               return (await _context.SaveChangesAsync())>0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RestaurantExists(restaurant.RestaurantId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Restaurant> GetRestaurantById(int restaurantId)
        {
            IQueryable<Restaurant> result = _context.Restaurants
                .Where(r => r.RestaurantId == restaurantId)
                .Include(r=>r.BookingSlots)
                .Include(r=>r.OwnerUser);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurants()
        {
            var result = _context.Restaurants
                .OrderBy(r => r.RestaurantId)
                .Include(r=>r.BookingSlots)
                .Include(r=>r.OwnerUser);
            return await result.ToListAsync();
        }

        public async Task<bool> RestaurantExists(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync<Restaurant>(r => r.RestaurantId == restaurantId);
        }

        public async Task<bool> SaveRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
