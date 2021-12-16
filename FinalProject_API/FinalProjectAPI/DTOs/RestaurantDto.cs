using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.DTOs
{
    public class RestaurantDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public string RestaurantCity { get; set; }
        public string CuisineType { get; set; }
        public int OwnerUserId { get; set; }
        public UserCredDto OwnerUser { get; set; }
        public ICollection<BookingSlotDto> BookingSlots { get; set; }
    }
}
