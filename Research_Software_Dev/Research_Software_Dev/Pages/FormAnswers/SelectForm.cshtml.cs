using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
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

        public List<Participant> FilteredParticipants { get; set; } = new();
        public List<Form> Forms { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("Session ID is required.");
            }

            SessionId = sessionId;

            // Authorization check
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") &&
                !roles.Contains("Mid-Auth") && !roles.Contains("Researcher") &&
                !roles.Contains("Low-Auth"))
            {
                return Forbid();
            }

            // Load participants for the selected session, ordered alphabetically
            FilteredParticipants = await _context.ParticipantSessions
                .Where(ps => ps.SessionId == sessionId)
                .Include(ps => ps.Participant)
                .Select(ps => ps.Participant)
                .OrderBy(p => p.ParticipantFirstName)
                .ThenBy(p => p.ParticipantLastName)
                .ToListAsync();

            // Load forms ordered by name
            Forms = await _context.Forms
                .OrderBy(f => f.FormName)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(FormId) || string.IsNullOrEmpty(ParticipantId))
            {
                ModelState.AddModelError(string.Empty, "Please select both a form and a participant.");
                return Page();
            }

            // Redirect to the answer submission page
            return RedirectToPage("./SubmitAnswers", new
            {
                formId = FormId,
                participantId = ParticipantId,
                sessionId = SessionId
            });
        }
    }
}
