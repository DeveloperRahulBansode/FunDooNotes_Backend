using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Context;
using DataAcessLayer.Entity;
using DataAcessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using ModelLayer;

namespace DataAcessLayer.Services
{
   public  class NotesDataService:INotesDataService
    {
        private readonly UserDBContext _userContext;

        public NotesDataService(UserDBContext userDBContext)
        {
            _userContext = userDBContext;

        }


        public async Task AddNotes(NotesModel model, int id)
        {
            var notes = new Notes
            {
                UserID = id,
                Title = model.Title,
                Description = model.Description,
                Color = model.Color,
                IsTrash = model.IsTrash,
                IsArchive = model.IsArchive
            };
            _userContext.Add(notes);
            await _userContext.SaveChangesAsync();

        }

        public async Task<Notes> CreateNoteWithLabel(NotesLabelModel model, int userId)
        {
            using var transaction = await _userContext.Database.BeginTransactionAsync(); 

            try
            {
                var note = new Notes
                {
                    UserID = userId,
                    Title = model.Title,
                    Description = model.Description,
                    Color = model.Color,
                    IsTrash = model.IsTrash,
                    IsArchive = model.IsArchive,
                    NoteLabels = new List<NoteLabel>()
                };

                _userContext.Notes.Add(note);
                await _userContext.SaveChangesAsync();

                if (model.LabelId != null && model.LabelId.Any())
                {
                    var labelIds = model.LabelId.Select(l => l).ToList();
                    var labels = await _userContext.Labels
                        .Where(l => labelIds.Contains(l.LabelId) && l.UserId == userId)
                        .ToListAsync();

                    if (!labels.Any())
                    {
                        return new Notes(); 
                    }

                    var noteLabels = labels.Select(label => new NoteLabel
                    {
                        NotesId = note.NotesId,
                        LabelId = label.LabelId
                    }).ToList();

                    _userContext.NoteLabels.AddRange(noteLabels);
                    await _userContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return note;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating note with labels: " + ex.Message);
            }
        }




        public async Task<bool> DeleteNote(int noteId, int userId)
        {
            var note = await _userContext.Notes
                .Where(n => n.NotesId == noteId && n.UserID == userId)
                .FirstOrDefaultAsync();

            if (note == null)
                return false;

            _userContext.Notes.Remove(note);
            await _userContext.SaveChangesAsync();
            return true;
        }


        public async Task<List<NotesLabelModel>> GetAllNotes(int userId)
        {
            var notes = await _userContext.Notes
        .Where(n => n.UserID == userId)
        .Include(n => n.NoteLabels)
        .Select(n => new NotesLabelModel
        {
            NoteId = n.NotesId,
            Title = n.Title,
            Description = n.Description,
            Color = n.Color,
            IsTrash = n.IsTrash,
            IsArchive = n.IsArchive,
            //LabelId = n.NoteLabels.Select(nl => nl.LabelId).ToList() 
        })
        .ToListAsync();

            return notes;
        }

        public async Task<NotesLabelModel> GetNoteById(int noteId, int userId)
        {
            var note = await _userContext.Notes
        .Where(n => n.NotesId == noteId && n.UserID == userId)
        .Include(n => n.NoteLabels)
        .Select(n => new NotesLabelModel
        {
            NoteId = n.NotesId,
            Title = n.Title,
            Description = n.Description,
            Color = n.Color,
            IsTrash = n.IsTrash,
            IsArchive = n.IsArchive,
            //LabelId = n.NoteLabels.Select(nl => nl.LabelId).ToList() 
        })
        .FirstOrDefaultAsync();

            return note;
        }





        public async Task<NotesLabelModel> UpdateNoteWithLabels(int noteId, int userId, NotesLabelModel updatedNote)
        {
            using var transaction = await _userContext.Database.BeginTransactionAsync();
            try
            {
                var note = await _userContext.Notes
                    .Include(n => n.NoteLabels)
                    .FirstOrDefaultAsync(n => n.NotesId == noteId && n.UserID == userId);

                if (note == null)
                    return null; 
                note.Title = updatedNote.Title;
                note.Description = updatedNote.Description;
                note.Color = updatedNote.Color;
                note.IsTrash = updatedNote.IsTrash;
                note.IsArchive = updatedNote.IsArchive;

                _userContext.NoteLabels.RemoveRange(note.NoteLabels);
                await _userContext.SaveChangesAsync();

                if (updatedNote.LabelId != null && updatedNote.LabelId.Any())
                {
                    var newNoteLabels = updatedNote.LabelId.Select(labelId => new NoteLabel
                    {
                        NotesId = note.NotesId,
                        LabelId = labelId 
                    }).ToList();

                    _userContext.NoteLabels.AddRange(newNoteLabels);
                }

                await _userContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new NotesLabelModel
                {
                    NoteId = note.NotesId,
                    Title = note.Title,
                    Description = note.Description,
                    Color = note.Color,
                    IsTrash = note.IsTrash,
                    IsArchive = note.IsArchive,
                    LabelId = _userContext.NoteLabels
                        .Where(nl => nl.NotesId == note.NotesId)
                        .Select(nl => nl.LabelId) 
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating note with labels: " + ex.Message);
            }
        }




    }
}
