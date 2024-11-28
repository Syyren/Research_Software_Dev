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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
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
                return Unauthorized();
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            var researcherStudy = await _context.ResearcherStudies
                .FirstOrDefaultAsync(rs => rs.StudyId == id && rs.ResearcherId == researcherId);

            if (researcherStudy == null)
            {
                return RedirectToPage("/NotFound");
            }

            //remove the association from ResearcherStudies
            _context.ResearcherStudies.Remove(researcherStudy);

            //remove the study
            var otherAssociationsExist = await _context.ResearcherStudies.AnyAsync(rs => rs.StudyId == id);
            if (!otherAssociationsExist)
            {
                var study = await _context.Studies.FindAsync(id);
                if (study != null)
                {
                    _context.Studies.Remove(study);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
