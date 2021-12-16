using AutoMapper;
using FinalProjectAPI.DTOs;
using FinalProjectLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectAPI.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>();
            CreateMap<BookingSlot, BookingSlotDto>();
            CreateMap<UserCred, UserCredDto>();
            CreateMap<Booking, BookingDto>();
        }
    }
}
