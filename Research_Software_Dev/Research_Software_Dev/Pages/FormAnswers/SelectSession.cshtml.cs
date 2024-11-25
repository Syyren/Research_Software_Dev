using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Pages.Forms
{
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

            // Load sessions linked to the participant
            Sessions = await _context.ParticipantSessions
                .Where(ps => ps.ParticipantId == participantId)
                .Include(ps => ps.Session)
                .Select(ps => ps.Session)
                .ToListAsync();

            if (!Sessions.Any())
            {
                // Handle the case where no sessions are found
                ModelState.AddModelError(string.Empty, "No sessions found for the selected participant.");
                return Page();
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
