using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace FinalProjectLibrary.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            BookingSlots = new HashSet<BookingSlot>();
        }
        [DisplayName("Restaurant Id")]
        public int RestaurantId { get; set; }
        [DisplayName("Restaurant Name")]
        public string RestaurantName { get; set; }
        [DisplayName("Restaurant Address")]
        public string RestaurantAddress { get; set; }
        [DisplayName("Restaurant City")]
        public string RestaurantCity { get; set; }
        [DisplayName("Cuisine Type")]
        public string CuisineType { get; set; }
        [DisplayName("Owner User Id")]
        public int OwnerUserId { get; set; }

        public virtual UserCred OwnerUser { get; set; }
        public virtual ICollection<BookingSlot> BookingSlots { get; set; }
    }
}
