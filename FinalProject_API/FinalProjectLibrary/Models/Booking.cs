using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace FinalProjectLibrary.Models
{
    public partial class Booking
    {
        [DisplayName("Booking Id")]
        public int BookingId { get; set; }
        [DisplayName("Number of Seats")]
        public int NumberOfSeats { get; set; }
        [DisplayName("Booking Slot Id")]
        public int BookingSlotId { get; set; }
        [DisplayName("User Id")]
        public int UserId { get; set; }

        public virtual BookingSlot BookingSlot { get; set; }
        public virtual UserCred User { get; set; }
        
        public IEnumerable<BookingSlot> BookingSlotsDropdown;
    }
}
