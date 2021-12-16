using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Service
{
    public interface IRestaurantRepository
    {
        Task<bool> RestaurantExists(int restaurantId);
        Task<IEnumerable<Restaurant>> GetRestaurants();
        Task<Restaurant> GetRestaurantById(int restaurantId);
        Task<bool> SaveRestaurant(Restaurant restaurant);
        Task<bool> EditRestaurant(Restaurant restaurant);
        Task<bool> DeleteRestaurant(int restaurantId);
    }
}
