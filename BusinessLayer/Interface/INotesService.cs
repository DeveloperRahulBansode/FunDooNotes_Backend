using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using ModelLayer;

namespace BusinessLayer.Interface
{
    public interface INotesService
    {
        Task<List<NotesLabelModel>> GetAllNotes(int userId);
        Task AddNotes(NotesModel model, int Id);
        Task<Notes> CreateNoteWithLabel(NotesLabelModel noteModel, int userId);
        Task<NotesLabelModel> GetNoteById(int noteId, int userId);
        //Task<NotesLabelModel> UpdateNoteWithLabels(int noteId, int userId, NotesLabelModel updatedNote);

        Task<NotesModel?> UpdateNote(int userId, int noteId, NotesModel updatedNote);
        Task<bool> DeleteNote(int noteId, int userId);
        Task<bool> ArchiveNote(int noteId, int userId);
        Task<bool> TrashNote(int noteId, int userId);

        Task<bool> ChangeNoteColor(int userId, int noteId, string newColor);
    }
}
