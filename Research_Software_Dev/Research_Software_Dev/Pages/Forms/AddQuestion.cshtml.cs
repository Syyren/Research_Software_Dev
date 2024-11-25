using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System;
using System.Linq;
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
            if (string.IsNullOrEmpty(formId))
            {
                return NotFound("FormId is missing.");
            }

            var form = await _context.Forms.FindAsync(formId);
            if (form == null)
            {
                return NotFound("Form not found.");
            }

            FormId = formId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Starting OnPostAsync...");

            if (string.IsNullOrEmpty(FormId))
            {
                Console.WriteLine("FormId is missing or empty.");
                ModelState.AddModelError(nameof(FormId), "FormId is required.");
                return Page();
            }

            Console.WriteLine($"FormId: {FormId}");
            Console.WriteLine($"QuestionDescription: {Question.QuestionDescription}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            // Assigns FormId explicitly
            Question.FormId = FormId;

            // Gets the next question number
            var lastQuestionNumber = await _context.FormQuestions
                .Where(q => q.FormId == FormId)
                .OrderByDescending(q => q.QuestionNumber)
                .Select(q => q.QuestionNumber)
                .FirstOrDefaultAsync();

            Question.QuestionNumber = lastQuestionNumber + 1;

            Console.WriteLine("Saving the question...");
            try
            {
                _context.FormQuestions.Add(Question);
                await _context.SaveChangesAsync();
                Console.WriteLine("Question saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving question: {ex.Message}");
                throw;
            }

            return RedirectToPage("./Edit", new { id = FormId });
        }
    }
}
