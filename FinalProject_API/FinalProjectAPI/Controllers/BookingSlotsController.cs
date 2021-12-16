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
    public class BookingSlotsController : ControllerBase
    {
        private IBookingSlotRepository _bookingSlotRepository;
        private readonly IMapper _mapper;

        public BookingSlotsController(IBookingSlotRepository bookingSlotRepository, IMapper mapper)
        {
            _bookingSlotRepository = bookingSlotRepository;
            _mapper = mapper;
        }

        // GET: api/BookingSlots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingSlot>>> GetBookingSlots()
        {
            var bookingSlots = await _bookingSlotRepository.GetBookingSlots();
            var results = _mapper.Map<IEnumerable<BookingSlotDto>>(bookingSlots);
            return Ok(results);
        }

        // GET: api/BookingSlots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingSlot>> GetBookingSlot(int id)
        {
            var bookingSlot = await _bookingSlotRepository.GetBookingSlotById(id);
            if (bookingSlot == null)
            {
                return NotFound();
            }

            var bookingSlotResult = _mapper.Map<BookingSlotDto>(bookingSlot);
            return Ok(bookingSlotResult);
        }

        // PUT: api/BookingSlots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookingSlot(int id, BookingSlot bookingSlot)
        {
            if (id != bookingSlot.BookingSlotId)
            {
                return BadRequest();
            }

            var result = await _bookingSlotRepository.EditBookingSlot(bookingSlot);
            if (!result)
            {
                return NotFound();
            }

            return CreatedAtAction("GetBookingSlot", new { id = bookingSlot.BookingSlotId }, bookingSlot);
        }

        // POST: api/BookingSlots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingSlot>> PostBookingSlot(BookingSlot bookingSlot)
        {
            var result = await _bookingSlotRepository.SaveBookingSlot(bookingSlot);

            return CreatedAtAction("GetBookingSlot", new { id = bookingSlot.BookingSlotId }, bookingSlot);
        }

        // DELETE: api/BookingSlots/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingSlot(int id)
        {
            var result = await _bookingSlotRepository.DeleteBookingSlot(id);
            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool BookingSlotExists(int id)
        {
            return _bookingSlotRepository.BookingSlotExists(id).Result;
        }
    }
}
