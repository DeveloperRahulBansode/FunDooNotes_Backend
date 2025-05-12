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

        [HttpGet("GetNoteById")]
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

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateNote( int noteId, NotesModel updatedNote)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _notesService.UpdateNote(noteId, userId, updatedNote);

            if (result == null)
                return NotFound(new { Message = "Note not found" });

            return Ok(result);
        }

        [HttpDelete("DeleteNote")]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool result = await _notesService.DeleteNote(noteId, userId);
            return result ? Ok("Deleted successfully") : NotFound("Note not found");
        }

        [HttpPut("Archive")]
        public async Task<IActionResult> ArchiveNote(int noteId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool result = await _notesService.ArchiveNote(noteId, userId);
            return result ? Ok("Archived successfully") : NotFound("Note not found");


        }

        [HttpPut("Trash")]
        public async Task<IActionResult> TrashNote(int noteId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool result = await _notesService.TrashNote(noteId, userId);
            return result ? Ok("Trashed successfully") : NotFound("Note not found");

        }

        [HttpPut("NoteColor")]
        public async Task<IActionResult> ChangeNoteColor( int noteId, string newColor)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool result = await _notesService.ChangeNoteColor(userId,noteId, newColor);
            return result ? Ok(new { message = "Note color changed successfully" }) : NotFound("Note not found");


        }
    }
}

