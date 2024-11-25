using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.ParticipantSessions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ParticipantSession ParticipantSession { get; set; } = default!;

        public IActionResult OnGet()
        {
            // Populate dropdowns
            ViewData["ParticipantId"] = new SelectList(
                _context.Participants.Select(p => new
                {
                    ParticipantId = p.ParticipantId,
                    FullName = p.ParticipantFirstName + " " + p.ParticipantLastName
                }),
                "ParticipantId",
                "FullName"
            );

            ViewData["SessionId"] = new SelectList(
                _context.Sessions.Select(s => new
                {
                    s.SessionId,
                    SessionDetails = s.Date.ToString("yyyy-MM-dd") + " (" + s.TimeStart + " - " + s.TimeEnd + ")"
                }),
                "SessionId",
                "SessionDetails"
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                // Re-populates dropdowns to avoid errors
                OnGet();
                return Page();
            }

            // Checks if the participant-session relationship already exists
            var existingSession = _context.ParticipantSessions
                .Any(ps => ps.ParticipantId == ParticipantSession.ParticipantId && ps.SessionId == ParticipantSession.SessionId);

            if (existingSession)
            {
                ModelState.AddModelError(string.Empty, "This participant is already assigned to the selected session.");
                OnGet();
                return Page();
            }

            _context.ParticipantSessions.Add(ParticipantSession);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
