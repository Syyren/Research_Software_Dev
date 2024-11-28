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
        private readonly string[] permissions = { "Study Admin", "High-Auth", "Mid-Auth"};

        public DetailsModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Participant Participant { get; set; } = default!;
        public string StudyName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            //gets the logged-in user's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            // Fetch roles and verify permissions
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
            if (!roles.Any(role => permissions.Contains(role)))
            {
                return Forbid();
            }

            if (id == null)
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
