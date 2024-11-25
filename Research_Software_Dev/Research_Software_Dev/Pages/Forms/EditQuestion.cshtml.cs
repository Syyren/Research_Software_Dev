using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    public class EditQuestionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditQuestionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public FormQuestion Question { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string questionId)
        {
            if (!ModelState.IsValid)
            {
                // Logs validation errors for debugging
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            Question = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormId == formId && q.FormQuestionId == questionId);

            if (Question == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Logs validation errors for debugging
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            // Finds the existing question in the database
            var existingQuestion = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormQuestionId == Question.FormQuestionId);

            if (existingQuestion == null)
            {
                return NotFound();
            }

            // Updates the question's properties
            existingQuestion.QuestionDescription = Question.QuestionDescription;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FormQuestions.Any(q => q.FormQuestionId == Question.FormQuestionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Edit", new { id = Question.FormId });
        }

    }
}