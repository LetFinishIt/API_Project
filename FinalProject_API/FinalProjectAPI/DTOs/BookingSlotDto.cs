using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.DTOs
{
    public class BookingSlotDto
    {
        public int BookingSlotId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SlotTime { get; set; }
        public int RestaurantId { get; set; }

        public RestaurantDto Restaurant { get; set; }
        public ICollection<BookingDto> Bookings { get; set; }
    }
}
