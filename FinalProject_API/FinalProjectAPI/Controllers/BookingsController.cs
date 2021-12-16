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
    public class BookingsController : ControllerBase
    {
        private IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly FinalProjectDBContext _context;

        public BookingsController(FinalProjectDBContext context,IBookingRepository bookingRepository, IMapper mapper)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            //return await _context.Bookings.ToListAsync();
            var bookings = await _bookingRepository.GetBookings();
            var results = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return Ok(results);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _bookingRepository.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            var bookingResult = _mapper.Map<BookingDto>(booking);
            return Ok(bookingResult);
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            var result = await _bookingRepository.EditBooking(booking);
            if (!result)
            {
                return NotFound();
            }

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            var result = await _bookingRepository.SaveBooking(booking);

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingRepository.DeleteBooking(id);
            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _bookingRepository.BookingExists(id).Result;
        }
    }
}
