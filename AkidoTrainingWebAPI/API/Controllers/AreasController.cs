using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AkidoTrainingWebAPI.DataAccess.Data;
using AkidoTrainingWebAPI.DataAccess.Models;
using AkidoTrainingWebAPI.BusinessLogic.DTOs.AreasDTO;
using AkidoTrainingWebAPI.BusinessLogic.Repositories;
using NuGet.Protocol.Core.Types;
using AkidoTrainingWebAPI.BusinessLogic.DTOs.AccountsDTO;
using System.Security.Principal;

namespace AkidoTrainingWebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly AkidoTrainingWebAPIContext _context;
        private readonly AreasRepository _repository;

        public AreasController(AkidoTrainingWebAPIContext context, AreasRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Areas>>> GetAreas()
        {
            return await _context.Areas.ToListAsync();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Areas>> GetAreas(int id)
        {
            var areas = await _context.Areas.FindAsync(id);

            if (areas == null)
            {
                return NotFound();
            }

            return areas;
        }

        // PUT: api/Areas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAreas(int id, AreasDTOPut area)
        {
            var areaToUpdate = await _context.Areas.FindAsync(id);
            if (areaToUpdate == null)
            {
                return BadRequest();
            }
            
            areaToUpdate.Name = area.Name;
            areaToUpdate.Description = area.Description;
            areaToUpdate.Address = area.Address;
            areaToUpdate.District = area.District;

            _context.Entry(areaToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreasExists(id))
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

        // POST: api/Areas
        [HttpPost]
        public async Task<ActionResult<Areas>> PostAreas([FromForm] AreasDTOAdd areas, IFormFile image)
        {
            var newArea = new AreasDTO
            {
                Name = areas.Name,
                District = areas.District,
                Address = areas.Address,
                Description = areas.Description,
            };

            newArea.ImagePath = await WriteFile(image, areas.Name, areas.District);
            //  await _repository.AddAccountsAsync(newAccount);
            await _repository.AddAreasAsync(newArea);
            // return CreatedAtAction(nameof(GetAccounts), new { id = newAccount.Id}, newAccount);
            return CreatedAtAction("GetAreas", new { id = newArea.Id }, newArea);
        }

        // DELETE: api/Areas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAreas(int id)
        {
            var areas = await _context.Areas.FindAsync(id);
            if (areas == null)
            {
                return NotFound();
            }
            DeleteAvatar(areas.ImagePath, areas.District);
            _context.Areas.Remove(areas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("Image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var area = await _context.Areas.FindAsync(id);

            if (area == null || string.IsNullOrEmpty(area.ImagePath))
            {
                return NotFound("Area or image not found.");
            }

            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "API", "Area", $"{area.District}");
            var imagePath = Path.Combine(uploadsDirectory, area.ImagePath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image not found.");
            }
            try
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving image: {ex.Message}");
            }
        }

        [HttpPut("EditImages/{id}")]
        public async Task<IActionResult> EditAreaImage(int id, IFormFile avatar)
        {
            var area = await _context.Areas.FindAsync(id);

            if (area == null)
            {
                return NotFound("Area not found");
            }
            DeleteAvatar(area.ImagePath, area.District);
            area.ImagePath = await WriteFile(avatar, area.Name, area.District);

            _context.Areas.Update(area);
            await _context.SaveChangesAsync();

            return Ok(area.ImagePath);
        }

        private async Task<string> WriteFile(IFormFile image, string? name, string? district)
        {
            string filename = "";
            try
            {
                var extension = Path.GetExtension(image.FileName);
                filename = name + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "API", "Area", $"{district}");
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);

                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing file: {ex.Message}");
            }
            return filename;
        }

        private void DeleteAvatar(string? imagePath, string? district)
        {
            try
            {
                var deleteFile = Path.Combine(Directory.GetCurrentDirectory(), "API", "Area", $"{district}", imagePath);
                if (System.IO.File.Exists(deleteFile))
                {
                    System.IO.File.Delete(deleteFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting avatar: {ex.Message}");
            }
        }

        private bool AreasExists(int id)
        {
            return _context.Areas.Any(e => e.Id == id);
        }
    }
}
