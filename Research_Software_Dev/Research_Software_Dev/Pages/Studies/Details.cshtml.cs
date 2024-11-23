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
    public class DetailsModel : PageModel
    {
        private readonly Research_Software_Dev.Data.ApplicationDbContext _context;

        public DetailsModel(Research_Software_Dev.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
