using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.DTOs
{
    public class UserCredDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public ICollection<BookingDto> Bookings { get; set; }
        public ICollection<RestaurantDto> Restaurants { get; set; }
    }
}
