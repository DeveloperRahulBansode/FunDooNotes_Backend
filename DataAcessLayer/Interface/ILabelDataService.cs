using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using ModelLayer;

namespace DataAcessLayer.Interface
{
    public interface ILabelDataService
    {
        Task<IEnumerable<Label>> GetAllLabels(int userId);
        Task<Label> GetLabelById(int labelId, int userId);
        Task<Label> CreateLabel(LabelModel label);
        Task<Label> UpdateLabel(LabelModel label, int userId);
        Task<bool> DeleteLabel(int labelId, int userId);

    }
}
