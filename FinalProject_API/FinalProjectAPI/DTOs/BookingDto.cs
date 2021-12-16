using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int NumberOfSeats { get; set; }
        public int BookingSlotId { get; set; }
        public int UserId { get; set; }
        public BookingSlotDto BookingSlot { get; set; }
        public UserCredDto User { get; set; }
    }
}
