using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAcessLayer.Entity;
using DataAcessLayer.Interface;
using ModelLayer;

namespace BusinessLayer.Service
{
    public class LabelService : ILabelService
    {
        public readonly ILabelDataService _labelDataService;

        public LabelService(ILabelDataService labelDataService)
        {
            _labelDataService = labelDataService;
        }

        public async Task<Label> CreateLabel(LabelModel label)
        {
            return await _labelDataService.CreateLabel(label);
        }

        public async Task<bool> DeleteLabel(int labelId, int userId)
        {
            return await (_labelDataService.DeleteLabel(labelId, userId));
        }

        public async Task<IEnumerable<Label>> GetAllLabels(int userId)
        {
            return await _labelDataService.GetAllLabels(userId);
        }

        public async Task<Label> GetLabelById(int labelId, int userId)
        {
            return await _labelDataService.GetLabelById(labelId, userId);
        }

        public async Task<Label> UpdateLabel(LabelModel label, int userId)
        {
            return await _labelDataService.UpdateLabel(label, userId);
        }
    }
}
