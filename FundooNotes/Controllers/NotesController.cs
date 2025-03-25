using System.Security.Claims;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        public readonly INotesService _notesService;
        public NotesController(INotesService notesService)
        {
            _notesService = notesService;

        }

        [HttpPost("AddNotes")]
        public async Task<IActionResult> AddNotes(NotesModel model)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _notesService.AddNotes(model, userId);
                return Ok(new { message = "Notes added successfully.." });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("createNoteWithLabels")]
        public async Task<IActionResult> CreateNoteWithLabels([FromBody] NotesLabelModel noteModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var note = await _notesService.CreateNoteWithLabel(noteModel, userId);
                return Ok(note);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("GetAllNotes")]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var notes = await _notesService.GetAllNotes(userId);
            return Ok(notes);


        }

        [HttpGet("{noteId}GetNoteById")]
        public async Task<IActionResult> GetNoteById(int noteId, int userId)
        {
            try
            {
                var note = await _notesService.GetNoteById(noteId, userId);
                return Ok(note);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("update-note/{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, int userId, [FromBody] NotesLabelModel updatedNote)
        {
            var result = await _notesService.UpdateNoteWithLabels(noteId, userId, updatedNote);

            if (result == null)
                return NotFound(new { Message = "Note not found" });

            return Ok(result); // ✅ Returns the updated note
        }

        [HttpDelete("{noteId}/DeleteNote")]
        public async Task<IActionResult> DeleteNote(int noteId, int userId)
        {
            bool result = await _notesService.DeleteNote(noteId, userId);
            return result ? Ok("Deleted successfully") : NotFound("Note not found");
        }
    }
}

