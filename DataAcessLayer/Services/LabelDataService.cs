using System;
using System.Collections.Generic;
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
    public class LabelDataService : ILabelDataService
    {
        public readonly UserDBContext _userDBContext;
        public LabelDataService(UserDBContext userDBContext)
        {
            _userDBContext = userDBContext;
        }
        public async Task<Label> CreateLabel(LabelModel label)
        {
            var existingLabel = await _userDBContext.Labels.FirstOrDefaultAsync(l => l.LabelName == label.LabelName && l.UserId == label.UserID);
            if (existingLabel != null)
            {
                throw new Exception("Label already exists");
            }

            var newLabel = new Label
            {
                LabelName = label.LabelName,
                UserId = label.UserID
            };

            _userDBContext.Labels.Add(newLabel);
            await _userDBContext.SaveChangesAsync();
            return newLabel;
        }

        public async Task<bool> DeleteLabel(int labelId, int userId)
        {
            var label = await _userDBContext.Labels.FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);
            if (label == null)
            {
                throw new Exception("Label not found");
            }

            _userDBContext.Labels.Remove(label);
            await _userDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Label>> GetAllLabels(int userId)
        {
            return await _userDBContext.Labels.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<Label> GetLabelById(int labelId, int userId)
        {
            var existingLabel = await _userDBContext.Labels.FirstOrDefaultAsync(l => l.LabelId == labelId && l.UserId == userId);
            if (existingLabel == null)
            {
                throw new Exception("Label not found");
            }
            return existingLabel;
        }

        public async Task<Label> UpdateLabel(LabelModel label, int userId)
        {
            var existingLabel = await _userDBContext.Labels.FirstOrDefaultAsync(l => l.LabelId == label.LabelId && l.UserId == userId);
            if (existingLabel == null)
            {
                throw new Exception("Label not found");
            }

            existingLabel.LabelName = label.LabelName;
            await _userDBContext.SaveChangesAsync();
            return existingLabel;
        }
    }
}
