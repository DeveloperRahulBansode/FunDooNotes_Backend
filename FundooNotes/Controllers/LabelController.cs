using System.Security.Claims;
using BusinessLayer.Interface;
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
    public class LabelController : ControllerBase
    {
        public readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {
            _labelService = labelService;
        }
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found");
            }
            return int.Parse(userIdClaim.Value);
        }


        [HttpGet("GetAllLabels")]
        public async Task<IActionResult> GetAllLabels()
        {
            int userId = GetUserIdFromToken();
            var labels = await _labelService.GetAllLabels(userId);
            return Ok(labels);
        }

        [HttpGet("{labelId}/GetLabelById")]
        public async Task<IActionResult> GetLabelById(int labelId)
        {
            int userId = GetUserIdFromToken();
            var label = await _labelService.GetLabelById(labelId, userId);
            return label == null ? NotFound() : Ok(label);
        }

        [HttpPost("CreateLabel")]
        public async Task<IActionResult> CreateLabel(LabelModel label)
        {
            label.UserID = GetUserIdFromToken();
            var createdLabel = await _labelService.CreateLabel(label);
            return CreatedAtAction(nameof(GetLabelById), new { labelId = createdLabel.LabelId }, createdLabel);
        }


        [HttpPut("UpdateLabel")]
        public async Task<IActionResult> UpdateLabel(LabelModel label)
        {
            label.UserID = GetUserIdFromToken();
            var updatedLabel = await _labelService.UpdateLabel(label, label.UserID);
            return Ok(updatedLabel);
        }

        [HttpDelete("{labelId}/DeleteLabel")]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            int userId = GetUserIdFromToken();
            await _labelService.DeleteLabel(labelId, userId);
            return NoContent();
        }
    }
}
