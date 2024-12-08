using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class SelectFormModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SelectFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public string SessionId { get; set; }

        public List<Form> Forms { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string participantId, string sessionId)
        {
            // Validate participant and session IDs
            if (string.IsNullOrEmpty(participantId) || string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("Participant ID and Session ID are required.");
            }

            ParticipantId = participantId;
            SessionId = sessionId;

            // Authorization roles
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("Study Admin") || roles.Contains("High-Auth") || roles.Contains("Mid-Auth") || roles.Contains("Researcher") || roles.Contains("Low-Auth"))
            {
                // Load forms sorted by name
                Forms = await _context.Forms.OrderBy(f => f.FormName).ToListAsync();
            }
            else
            {
                return Forbid();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            // Validate form selection
            if (string.IsNullOrEmpty(FormId))
            {
                ModelState.AddModelError(string.Empty, "Please select a form.");
                return Page();
            }

            // Redirect to the answer submission page
            return RedirectToPage("./SubmitAnswers", new { formId = FormId, participantId = ParticipantId, sessionId = SessionId });
        }
    }
}
