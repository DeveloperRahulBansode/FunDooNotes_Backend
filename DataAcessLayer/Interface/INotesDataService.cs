using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using ModelLayer;

namespace DataAcessLayer.Interface
{
   public interface INotesDataService
    {
        Task<List<NotesLabelModel>> GetAllNotes(int userId);   
        Task AddNotes(NotesModel model, int Id);
        Task<Notes> CreateNoteWithLabel(NotesLabelModel noteModel, int userId);
        Task<NotesLabelModel> GetNoteById(int noteId, int userId);
        Task<NotesLabelModel> UpdateNoteWithLabels(int noteId, int userId, NotesLabelModel updatedNote);
        Task<bool> DeleteNote(int noteId, int userId);
    }
}
