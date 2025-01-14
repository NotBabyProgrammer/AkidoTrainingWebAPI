﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AkidoTrainingWebAPI.BusinessLogic.DTOs.DistrictsDTO;
using AkidoTrainingWebAPI.DataAccess.Data;
using AkidoTrainingWebAPI.DataAccess.Models;

namespace AkidoTrainingWebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly AkidoTrainingWebAPIContext _context;

        public DistrictsController(AkidoTrainingWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Districts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Districts>>> GetDistricts()
        {
            return await _context.Districts.ToListAsync();
        }

        // GET: api/Districts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Districts>> GetDistricts(int id)
        {
            var districts = await _context.Districts.FindAsync(id);

            if (districts == null)
            {
                return NotFound();
            }

            return districts;
        }

        // PUT: api/Districts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistricts(int id,DistrictsDTO districts)
        {
            var districtsToUpdate = await _context.Accounts.FindAsync(id);
            if (districtsToUpdate == null)
            {
                return BadRequest();
            }

            _context.Entry(districts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Districts
        [HttpPost]
        public async Task<ActionResult<Districts>> PostDistricts(Districts districts)
        {
            _context.Districts.Add(districts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistricts", new { id = districts.Id }, districts);
        }

        // DELETE: api/Districts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistricts(int id)
        {
            var districts = await _context.Districts.FindAsync(id);
            if (districts == null)
            {
                return NotFound();
            }

            _context.Districts.Remove(districts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistrictsExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }
    }
}
