using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class SelectSessionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SelectSessionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SessionId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        public List<Participant> FilteredParticipants { get; set; } = new List<Participant>();
        public List<Session> Sessions { get; set; } = new List<Session>();
        public SelectList SessionOptions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Any(role => role.Contains("Auth") || role == "Researcher" || role == "Study Admin"))
            {
                return Forbid();
            }

            Sessions = await _context.Sessions.ToListAsync();
            SessionOptions = new SelectList(Sessions, "SessionId", "Date");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(SessionId))
            {
                ModelState.AddModelError(string.Empty, "Please select a session.");
                return await OnGetAsync(); // Reload sessions if none selected.
            }

            FilteredParticipants = await _context.ParticipantSessions
                .Where(ps => ps.SessionId == SessionId)
                .Include(ps => ps.Participant)
                .Select(ps => ps.Participant)
                .ToListAsync();

            Sessions = await _context.Sessions.ToListAsync();
            SessionOptions = new SelectList(Sessions, "SessionId", "Date");

            if (!FilteredParticipants.Any())
            {
                ModelState.AddModelError(string.Empty, "No participants found for the selected session.");
                return Page();
            }

            if (!string.IsNullOrEmpty(ParticipantId))
            {
                return RedirectToPage("./SelectForm", new { sessionId = SessionId, participantId = ParticipantId });
            }

            return Page();
        }
    }
}
