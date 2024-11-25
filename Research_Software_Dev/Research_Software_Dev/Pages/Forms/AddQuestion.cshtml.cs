using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    public class AddQuestionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddQuestionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FormQuestion Question { get; set; }

        [BindProperty]
        public string FormId { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId)
        {
            // Checks if the form exists
            var form = await _context.Forms.FindAsync(formId);
            if (form == null)
            {
                return NotFound();
            }

            FormId = formId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            // Sets the FormId for the new question
            Question.FormId = FormId;

            // Determine the next question number
            var lastQuestionNumber = await _context.FormQuestions
                .Where(q => q.FormId == FormId)
                .OrderByDescending(q => q.QuestionNumber)
                .Select(q => q.QuestionNumber)
                .FirstOrDefaultAsync();

            Question.QuestionNumber = lastQuestionNumber + 1;

            // Adds the question to the database
            _context.FormQuestions.Add(Question);
            await _context.SaveChangesAsync();

            // Redirects back to the Edit page for the form
            return RedirectToPage("./Edit", new { id = FormId });
        }
    }
}