using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAcessLayer.Entity;
using DataAcessLayer.Interface;
using ModelLayer;

namespace BusinessLayer.Service
{
    public class NotesService : INotesService

    {
        public readonly INotesDataService _dataService;

        public NotesService(INotesDataService notesDataService)
        {
            _dataService = notesDataService;
        }

        public async Task AddNotes(NotesModel model, int Id)
        {
            await _dataService.AddNotes(model, Id);
        }

        public async Task<bool> ArchiveNote(int noteId, int userId)
        {
            return await _dataService.ArchiveNote(noteId, userId);
            
        }

        public async Task<bool> ChangeNoteColor(int userId, int noteId, string newColor)
        {
            return  await _dataService.ChangeNoteColor(userId, noteId, newColor);

        }

        public async Task<Notes> CreateNoteWithLabel(NotesLabelModel noteModel, int userId)
        {
            return await _dataService.CreateNoteWithLabel(noteModel, userId);

        }

        public async Task<bool> DeleteNote(int noteId, int userId)
        {
            return await _dataService.DeleteNote(noteId, userId);
        }

        public async Task<List<NotesLabelModel>> GetAllNotes(int userId)
        {
            return await _dataService.GetAllNotes(userId);
        }

        public async Task<NotesLabelModel> GetNoteById(int noteId, int userId)
        {
            return await _dataService.GetNoteById(noteId, userId);
        }

        public async Task<bool> TrashNote(int noteId, int userId)
        {
            return await _dataService.TrashNote(noteId,userId);
            
        }

        public async Task<NotesModel?> UpdateNote(int userId, int noteId, NotesModel updatedNote)
        {
            return await _dataService.UpdateNote(noteId, userId, updatedNote);
        }
    }
}
