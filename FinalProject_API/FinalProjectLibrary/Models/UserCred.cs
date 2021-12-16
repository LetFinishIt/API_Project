using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace FinalProjectLibrary.Models
{
    public partial class UserCred
    {
        public UserCred()
        {
            Bookings = new HashSet<Booking>();
            Restaurants = new HashSet<Restaurant>();
        }
        [DisplayName("User Id")]
        public int UserId { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
