using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Studies;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Data
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedStudyId { get; set; }

        public List<Study> AvailableStudies { get; set; }

        public async Task OnGetAsync()
        {
            // Get the current researcher's ID
            var researcherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (researcherId == null)
            {
                // If the researcher is not logged in or not identified, return no studies
                AvailableStudies = new List<Study>();
                return;
            }

            // Fetch studies associated with the current researcher
            AvailableStudies = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Select(rs => rs.Study)
                .OrderBy(s => s.StudyName)
                .ToListAsync();

            // Reset SelectedStudyId if it doesn't match an available study
            if (!string.IsNullOrEmpty(SelectedStudyId) && !AvailableStudies.Any(s => s.StudyId == SelectedStudyId))
            {
                SelectedStudyId = null;
            }

            // Debugging
            Console.WriteLine($"Researcher ID: {researcherId}");
            Console.WriteLine($"Available Studies: {string.Join(", ", AvailableStudies.Select(s => s.StudyName))}");
        }
    }
}
