using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Form Form { get; set; }
        public List<FormQuestion> Questions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Form = await _context.Forms.FindAsync(id);
            if (Form == null)
            {
                return NotFound();
            }

            Questions = await _context.FormQuestions
                .Where(q => q.FormId == id)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            return Page();
        }
    }
}