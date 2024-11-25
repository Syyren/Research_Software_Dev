using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Sessions;
using Research_Software_Dev.Models.Studies;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Sessions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Session Session { get; set; } = default!;

        public IActionResult OnGet()
        {
            // Get the current logged-in researcher's ID
            var researcherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(researcherId))
            {
                Console.WriteLine("Researcher ID not found.");
                return RedirectToPage("/Error");
            }

            Console.WriteLine($"Researcher ID: {researcherId}");

            // Step 1: Retrieve all StudyIds for the logged-in researcher
            var studyIds = _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Select(rs => rs.StudyId)
                .ToList();

            Console.WriteLine($"Found StudyIds: {string.Join(", ", studyIds)}");

            // Step 2: Use StudyIds to fetch the associated studies
            var studies = _context.Studies
                .Where(s => studyIds.Contains(s.StudyId))
                .ToList();

            Console.WriteLine($"Found Studies: {string.Join(", ", studies.Select(s => s.StudyName))}");

            if (!studies.Any())
            {
                Console.WriteLine("No studies found for the researcher.");
                ViewData["StudyId"] = new SelectList(Enumerable.Empty<Study>(), "StudyId", "StudyName");
                ModelState.AddModelError("", "No studies are associated with your account.");
                return Page();
            }

            // Populate the dropdown with the filtered studies
            ViewData["StudyId"] = new SelectList(studies, "StudyId", "StudyName");

            // Initialize the session object
            Session = new Session
            {
                SessionId = Guid.NewGuid().ToString(),
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            return Page();
        }




        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulates dropdown if validation fails
                ViewData["StudyId"] = new SelectList(await _context.Studies.ToListAsync(), "StudyId", "StudyName");
                return Page();
            }

            // Explicitly checks StudyId and handle validation
            if (string.IsNullOrEmpty(Session.StudyId))
            {
                ModelState.AddModelError("Session.StudyId", "StudyId is required.");
                ViewData["StudyId"] = new SelectList(await _context.Studies.ToListAsync(), "StudyId", "StudyName");
                return Page();
            }

            // Adds session to database
            _context.Sessions.Add(Session);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
