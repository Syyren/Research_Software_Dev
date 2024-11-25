using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Sessions;
using System;
using System.Linq;
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
            // Populate StudyId dropdown with studies
            ViewData["StudyId"] = new SelectList(_context.Studies, "StudyId", "StudyName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdown if validation fails
                ViewData["StudyId"] = new SelectList(await _context.Studies.ToListAsync(), "StudyId", "StudyName");
                return Page();
            }

            // Explicitly check StudyId and handle validation
            if (string.IsNullOrEmpty(Session.StudyId))
            {
                ModelState.AddModelError("Session.StudyId", "StudyId is required.");
                ViewData["StudyId"] = new SelectList(await _context.Studies.ToListAsync(), "StudyId", "StudyName");
                return Page();
            }

            // Add session to database
            _context.Sessions.Add(Session);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
