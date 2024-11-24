using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<IActionResult> OnGetAsync(int formId)
        {
            // Fetch the form details by ID
            Form = await _context.Forms.FindAsync(formId);

            if (Form == null)
            {
                return NotFound();
            }

            // Fetch and sort the associated questions by QuestionNumber
            Questions = _context.FormQuestions
                .Where(q => q.FormId == formId)
                .OrderBy(q => q.QuestionNumber) // Sort by QuestionNumber ascending
                .ToList();

            return Page();
        }
    }
}