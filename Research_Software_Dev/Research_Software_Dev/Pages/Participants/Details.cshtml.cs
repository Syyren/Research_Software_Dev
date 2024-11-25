
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;

namespace Research_Software_Dev.Pages.Participants
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

        public Participant Participant { get; set; } = default!;
        public string StudyName { get; set; } = string.Empty;

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

            //check if participant and researcher is in same study
            var participantStudy = await _context.ParticipantStudies
                .Include(ps => ps.Participant)
                .Include(ps => ps.Study) //include Study for StudyName
                .FirstOrDefaultAsync(ps => ps.ParticipantId == id
                    && _context.ResearcherStudies.Any(rs => rs.StudyId == ps.StudyId && rs.ResearcherId == researcherId));

            if (participantStudy == null)
            {
                return RedirectToPage("/NotFound");
            }
            Participant = participantStudy.Participant;
            StudyName = participantStudy.Study.StudyName;
            return Page();
        }
    }
}
