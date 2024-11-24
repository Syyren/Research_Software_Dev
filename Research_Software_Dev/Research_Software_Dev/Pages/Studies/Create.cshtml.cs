using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Studies
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Researcher> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<Researcher> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Study Study { get; set; } = new Study();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //generates a unique StudyId using GUID
            Study.StudyId = Guid.NewGuid().ToString();

            //gets the logged-in user's ID (ResearcherId)
            var researcherId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(researcherId))
            {
                return RedirectToPage("/NotFound");
            }

            //creates a ResearcherStudy entry to associate the study with the researcher
            var researcherStudy = new ResearcherStudy
            {
                ResearcherId = researcherId,
                StudyId = Study.StudyId,
                Study = Study
            };

            //adds the new study and researcher-study link to the database
            _context.Studies.Add(Study); // Add the Study
            _context.ResearcherStudies.Add(researcherStudy); // Add the ResearcherStudy link
            await _context.SaveChangesAsync(); // Save to the database

            return RedirectToPage("./Index");
        }

    }
}