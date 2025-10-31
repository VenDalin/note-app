using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.DTOs;
using NotesApi.Models;
using NotesApi.Repositories; 
using System.Security.Claims;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _repo;
        
        public NotesController(INoteRepository repo) => _repo = repo;

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] NoteQueryParams p)
        {
            try
            {
                var userId = GetUserId();
                var result = await _repo.QueryNotesAsync(userId, p);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var userId = GetUserId();
                var note = await _repo.GetByIdAsync(id, userId);
                if (note == null) return NotFound("Note not found");
                return Ok(note);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Note note)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(note.Title))
                    return BadRequest("Title required");
                
                note.UserId = GetUserId();
                var id = await _repo.CreateAsync(note);
                
                var createdNote = await _repo.GetByIdAsync(id, note.UserId);
                if (createdNote == null) return StatusCode(500, "Failed to retrieve created note");
                
                return CreatedAtAction(nameof(Get), new { id = id }, createdNote);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Note note)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(note.Title))
                    return BadRequest("Title required");
                
                note.Id = id;
                note.UserId = GetUserId();
                
                var existing = await _repo.GetByIdAsync(id, note.UserId);
                if (existing == null) return NotFound("Note not found");
                
                await _repo.UpdateAsync(note);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();
                
                var existing = await _repo.GetByIdAsync(id, userId);
                if (existing == null) return NotFound("Note not found");
                
                await _repo.DeleteAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}