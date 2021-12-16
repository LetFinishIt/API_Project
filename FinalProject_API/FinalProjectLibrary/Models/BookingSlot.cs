using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FinalProjectLibrary.Models
{
    public partial class BookingSlot
    {
        public BookingSlot()
        {
            Bookings = new HashSet<Booking>();
        }
        [DisplayName("Booking Slot Id")]
        public int BookingSlotId { get; set; }
        [DisplayName("Slot Time")]
        [DataType(DataType.DateTime)]
        public DateTime SlotTime { get; set; }
        [DisplayName("Restaurant Id")]
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public IEnumerable<Restaurant> RestaurantsDropdown;
    }
}
