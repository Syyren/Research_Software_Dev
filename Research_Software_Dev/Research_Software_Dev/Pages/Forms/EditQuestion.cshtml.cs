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
        public int FormId { get; set; }

        [BindProperty]
        public FormQuestion Question { get; set; }

        public async Task<IActionResult> OnGetAsync(int formId, string questionId)
        {
            FormId = formId;

            Question = await _context.FormQuestions.FindAsync(questionId);

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
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(",", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            // Fetches the existing question from the database
            var existingQuestion = await _context.FormQuestions.FindAsync(Question.QuestionId);

            if (existingQuestion == null)
            {
                return NotFound(); // Question does not exist
            }

            // Updates the question properties
            existingQuestion.QuestionDescription = Question.QuestionDescription;
            existingQuestion.QuestionNumber = Question.QuestionNumber;

            // Marks the entity as modified
            _context.Attach(existingQuestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Saves changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FormQuestions.Any(q => q.QuestionId == Question.QuestionId))
                {
                    return NotFound(); // Handles concurrency issue if the record no longer exists
                }
                else
                {
                    throw; // Rethrows other concurrency exceptions
                }
            }

            // Redirects back to the form's edit page
            return RedirectToPage("./Edit", new { id = FormId });
        }
    }
}