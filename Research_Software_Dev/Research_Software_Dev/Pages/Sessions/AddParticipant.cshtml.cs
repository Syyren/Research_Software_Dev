using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.ParticipantSessions
{
    public class AddParticipantModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddParticipantModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SessionId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        public SelectList ParticipantList { get; set; }

        public IActionResult OnGet(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return NotFound("Session ID is required.");
            }

            SessionId = sessionId;

            // Populates the participant dropdown using SelectList
            ParticipantList = new SelectList(
                _context.Participants.ToList(),
                "ParticipantId",
                "ParticipantFirstName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Re-populates the dropdown if the form is invalid
                ParticipantList = new SelectList(
                    _context.Participants.ToList(),
                    "ParticipantId",
                    "ParticipantFirstName");

                return Page();
            }

            var participantSession = new ParticipantSession
            {
                ParticipantId = ParticipantId,
                SessionId = SessionId
            };

            _context.ParticipantSessions.Add(participantSession);
            await _context.SaveChangesAsync();

            // Redirects back to Sessions Index after adding the participant
            return RedirectToPage("/Sessions/Index");
        }
    }
}
