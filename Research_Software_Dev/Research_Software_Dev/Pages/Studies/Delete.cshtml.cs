using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Pages.Studies
{
    public class DeleteModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DeleteModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Study Study { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var study = await _context.Study.FirstOrDefaultAsync(m => m.StudyId == id);

            if (study == null)
            {
                return NotFound();
            }
            else
            {
                Study = study;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var study = await _context.Study.FindAsync(id);
            if (study != null)
            {
                Study = study;
                _context.Study.Remove(Study);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
