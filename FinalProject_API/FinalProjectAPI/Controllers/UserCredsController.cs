using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProjectLibrary.Models;
using AutoMapper;
using FinalProjectAPI.Service;
using FinalProjectAPI.DTOs;

namespace FinalProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCredsController : ControllerBase
    {
        private IUserCredRepository _userCredRepository;
        private readonly IMapper _mapper;

        public UserCredsController(IUserCredRepository userCredRepository,IMapper mapper)
        {
            _userCredRepository = userCredRepository;
            _mapper = mapper;
        }

        // GET: api/UserCreds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCred>>> GetUserCreds()
        {
            var userCreds = await _userCredRepository.GetUserCreds();
            var results = _mapper.Map<IEnumerable<UserCredDto>>(userCreds);
            return Ok(results);
        }

        // GET: api/UserCreds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCred>> GetUserCred(int id)
        {
            var userCred = await _userCredRepository.GetUserCredById(id);
            if (userCred == null)
            {
                return NotFound();
            }

            var userCredResult = _mapper.Map<UserCredDto>(userCred);
            return Ok(userCredResult);
        }

        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<UserCred>> GetSigninUser(string email, string password)
        {
            var userCred = await _userCredRepository.GetUserByEmailPassword(email,password);
            if (userCred == null)
            {
                return NotFound();
            }

            var userCredResult = _mapper.Map<UserCredDto>(userCred);
            return Ok(userCredResult);
        }

        // PUT: api/UserCreds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCred(int id, UserCred userCred)
        {
            if (id != userCred.UserId)
            {
                return BadRequest();
            }

            var result = await _userCredRepository.EditUserCred(userCred);
            if (!result)
            {
                return NotFound();
            }

            return CreatedAtAction("GetUserCred", new { id = userCred.UserId }, userCred);
        }

        // POST: api/UserCreds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserCred>> PostUserCred(UserCred userCred)
        {
            var result = await _userCredRepository.SaveUserCred(userCred);

            return CreatedAtAction("GetUserCred", new { id = userCred.UserId }, userCred);
        }

        // DELETE: api/UserCreds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCred(int id)
        {
            var result = await _userCredRepository.DeleteUserCred(id);
            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool UserCredExists(int id)
        {
            return _userCredRepository.UserCredExists(id).Result;
        }
    }
}
