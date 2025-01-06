using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AkidoTrainingWebAPI.BusinessLogic.DTOs;
using AkidoTrainingWebAPI.BusinessLogic.Repositories;
using AkidoTrainingWebAPI.DataAccess.Data;
using AkidoTrainingWebAPI.DataAccess.Models;
using AkidoTrainingWebAPI.BusinessLogic.DTOs.ContentsDTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AkidoTrainingWebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        private readonly AkidoTrainingWebAPIContext _context;

        public ContentsController(AkidoTrainingWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Contents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contents>>> GetContents()
        {
            return await _context.Contents.ToListAsync();
        }

        // GET: api/Contents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contents>> GetContents(int id)
        {
            var contents = await _context.Contents.FindAsync(id);

            if (contents == null)
            {
                return NotFound();
            }

            return contents;
        }
        // GET: api/Contents/Post/5
        [HttpGet("Post/{postId}")]
        public async Task<ActionResult<IEnumerable<Contents>>> GetContentsFilledByPostId(int postId)
        {
            if (!_context.Posts.Any(p => postId == p.Id))
            {
                return NotFound("This post is not existing");
            }

            var contents = await _context.Contents
                                         .Where(c => c.PostId == postId)
                                         .OrderBy(c => c.Order)
                                         .ToListAsync();

            if (contents == null || !contents.Any())
            {
                return NotFound($"No contents found for PostId: {postId}");
            }

            return Ok(contents);
        }


        // PUT: api/Contents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContents(int id, Contents contents)
        {
            if (id != contents.Id)
            {
                return BadRequest();
            }

            _context.Entry(contents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContentsExists(id))
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

        // POST: api/Contents/AddText
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddText")]
        public async Task<ActionResult<Contents>> AddText(ContentsDTOText contents)
        {
            if(!_context.Posts.Any(p => contents.PostId == p.Id))
            {
                return NotFound("This post is not existing");
            }

            int newOrder;
            if (_context.Contents.Any(c => c.PostId == contents.PostId))
            {
                newOrder = _context.Contents
                                   .Where(c => c.PostId == contents.PostId)
                                   .Max(c => c.Order) + 1;
            }
            else
            {
                newOrder = 1;
            }

            var newContent = new Contents
            {
                PostId = contents.PostId,
                Content = contents.Text,
                Order = newOrder
            };

            _context.Contents.Add(newContent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContents", new { id = newContent.Id }, newContent);
        }

        //POST: api/Contents/AddVideo
        // To protect con cac
        [HttpPost("AddVideo")]
        public async Task<ActionResult<Contents>> AddVideo(ContentsDTOVideos contents)
        {
            if (!_context.Posts.Any(p => contents.PostId == p.Id))
            {
                return NotFound("This post is not existing");
            }

            int newOrder;
            if (_context.Contents.Any(c => c.PostId == contents.PostId))
            {
                newOrder = _context.Contents
                                   .Where(c => c.PostId == contents.PostId)
                                   .Max(c => c.Order) + 1;
            }
            else
            {
                newOrder = 1;
            }

            var newContent = new Contents
            {
                PostId = contents.PostId,
                //Content = contents.Text,
                Order = newOrder
            };

            return CreatedAtAction("GetContents", new { id = newContent.Id }, newContent);
        }

        // DELETE: api/Contents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContents(int id)
        {
            var contents = await _context.Contents.FindAsync(id);
            if (contents == null)
            {
                return NotFound();
            }

            _context.Contents.Remove(contents);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContentsExists(int id)
        {
            return _context.Contents.Any(e => e.Id == id);
        }
    }
}
