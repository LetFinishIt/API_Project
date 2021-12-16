using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProjectLibrary.Models;
using FinalProjectAPI.Service;
using AutoMapper;
using FinalProjectAPI.DTOs;

namespace FinalProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        // GET: api/Restaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            var restaurants = await _restaurantRepository.GetRestaurants();
            var results = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
            return Ok(results);
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantRepository.GetRestaurantById(id);
            if (restaurant == null) 
            { 
                return NotFound(); 
            }
            
            var restaurantResult = _mapper.Map<RestaurantDto>(restaurant);
            return Ok(restaurantResult);
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(int id, Restaurant restaurant)
        {
            if (id != restaurant.RestaurantId)
            {
                return BadRequest();
            }

            var result=await _restaurantRepository.EditRestaurant(restaurant);
            if (!result)
            {
                return NotFound();
            }

            return CreatedAtAction("GetRestaurant", new { id = restaurant.RestaurantId }, restaurant);
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant(Restaurant restaurant)
        {
            var result = await _restaurantRepository.SaveRestaurant(restaurant);

            return CreatedAtAction("GetRestaurant", new { id = restaurant.RestaurantId }, restaurant);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var result = await _restaurantRepository.DeleteRestaurant(id);
            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool RestaurantExists(int id)
        {
            return _restaurantRepository.RestaurantExists(id).Result;
        }
    }
}
