using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Pdf
{
    public class ResultsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Form Form { get; set; }
        public List<FormQuestion> Questions { get; set; }

        public ResultsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string formId)
        {
            // Fetches the form details by ID
            Form = await _context.Forms.FindAsync(formId);

            if (Form == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Fetches and sorts the associated questions by QuestionNumber
            Questions = await _context.FormQuestions
                .Where(q => q.FormId == formId)
                .OrderBy(q => q.QuestionNumber) // Sorts by QuestionNumber ascending
                .ToListAsync();

            return Page();
        }
    }
}