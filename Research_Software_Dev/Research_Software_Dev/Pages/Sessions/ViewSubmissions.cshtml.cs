using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Sessions
{
    public class ViewSubmissionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewSubmissionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SessionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ParticipantId { get; set; }

        public SelectList ParticipantOptions { get; set; }
        public List<FormAnswer> FormAnswers { get; set; }

        public async Task OnGetAsync(string sessionId, string participantId)
        {
            SessionId = sessionId;

            // Fetch participants for the session using the ParticipantSessions table
            var participantIds = await _context.ParticipantSessions
                .Where(ps => ps.SessionId == sessionId)
                .Select(ps => ps.ParticipantId)
                .ToListAsync();

            var participants = await _context.Participants
                .Where(p => participantIds.Contains(p.ParticipantId))
                .ToListAsync();

            ParticipantOptions = new SelectList(participants, "ParticipantId", "ParticipantFirstName");

            // Fetch and order form answers for the selected participant in the selected session
            if (!string.IsNullOrEmpty(participantId))
            {
                ParticipantId = participantId;
                FormAnswers = await _context.FormAnswers
                    .Where(fa => fa.ParticipantId == participantId && fa.SessionId == sessionId)
                    .Include(fa => fa.FormQuestion)
                    .OrderBy(fa => fa.FormQuestion.QuestionNumber)
                    .ToListAsync();
            }
        }
    }
}
