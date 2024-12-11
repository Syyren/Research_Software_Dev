using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string ParticipantId { get; set; }

        [BindProperty]
        public string SessionId { get; set; }
        public string ParticipantName { get; set; }

        public List<Session> Sessions { get; set; } = new List<Session>();

        public async Task<IActionResult> OnGetAsync(string participantId)
        {
            ParticipantId = participantId;

            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("Study Admin") || roles.Contains("High-Auth") || roles.Contains("Mid-Auth") || roles.Contains("Researcher") || roles.Contains("Low-Auth"))
            {
                Sessions = await _context.ParticipantSessions
                    .Where(ps => ps.ParticipantId == participantId)
                    .Include(ps => ps.Session)
                    .Select(ps => ps.Session)
                    .ToListAsync();

                if (!Sessions.Any())
                {
                    ModelState.AddModelError(string.Empty, "No sessions found for the selected participant.");
                    return Page();
                }
            }
            else
            {
                return Forbid();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SessionId))
            {
                ModelState.AddModelError(string.Empty, "Please select a session.");
                return Page();
            }

            return RedirectToPage("./SelectForm", new { participantId = ParticipantId, sessionId = SessionId });
        }
    }
}
