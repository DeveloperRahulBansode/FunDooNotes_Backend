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

        public async Task<NotesLabelModel> UpdateNoteWithLabels(int noteId, int userId, NotesLabelModel updatedNote)
        {
            return await _dataService.UpdateNoteWithLabels(noteId, userId, updatedNote);
        }
    }
}
