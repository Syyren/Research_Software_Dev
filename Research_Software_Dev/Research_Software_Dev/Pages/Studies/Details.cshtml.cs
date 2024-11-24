using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Researchers;
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
            return Page();
        }
    }
}
