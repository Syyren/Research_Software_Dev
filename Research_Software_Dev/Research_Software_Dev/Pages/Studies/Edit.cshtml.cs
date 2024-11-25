using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Studies
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<Researcher> userManager)
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

            //fetches the study and verify ownership
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //gets the logged-in user's ID
            var researcherId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            //verifies ownership before updating
            var researcherStudy = await _context.ResearcherStudies
                .FirstOrDefaultAsync(rs => rs.StudyId == Study.StudyId && rs.ResearcherId == researcherId);

            if (researcherStudy == null)
            {
                return RedirectToPage("/NotFound");
            }

            //attaches and modifies the study
            _context.Attach(Study).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyExists(Study.StudyId))
                {
                    return RedirectToPage("/NotFound");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StudyExists(string id)
        {
            return _context.Studies.Any(e => e.StudyId == id);
        }
    }
}