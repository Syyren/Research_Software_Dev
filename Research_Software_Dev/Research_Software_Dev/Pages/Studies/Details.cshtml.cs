using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Sessions;
using Research_Software_Dev.Models.Studies;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Studies
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Study Study { get; set; } = default!;
        public List<Participant> Participants { get; set; }
        public List<Session> Sessions { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            //gets the logged-in user's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            //fetches the study and verify it belongs to the user
            var researcherStudy = await _context.ResearcherStudies
                .Include(rs => rs.Study)
                .FirstOrDefaultAsync(rs => rs.StudyId == id && rs.ResearcherId == researcherId);

            if (researcherStudy == null)
            {
                return RedirectToPage("/NotFound");
            }

            Study = researcherStudy.Study;

            //grabbing a list of participants by their ids and then attached sessions
            var participantIds = _context.ParticipantStudies
                .Where(ps => ps.StudyId == Study.StudyId)
                .Select(ps => ps.ParticipantId)
                .ToList();
            Participants = _context.Participants
                .Where(p => participantIds.Contains(p.ParticipantId))
                .OrderBy(p => p.ParticipantFirstName)
                .ToList();

            //grabbing a list of sessions by their study id
            Sessions = _context.Sessions
                .Where(s => s.StudyId == Study.StudyId)
                .OrderBy(s => s.Date)
                .ToList();

            return Page();
        }
    }
}
