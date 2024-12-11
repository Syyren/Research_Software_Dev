using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Collections.Generic;
using System.Linq;
<<<<<<< Updated upstream
=======
using System.Security.Claims;
>>>>>>> Stashed changes
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class AddQuestionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddQuestionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FormQuestion Question { get; set; } = new();

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
<<<<<<< Updated upstream
        public List<FormQuestionOption> Choices { get; set; } = new();
=======
        public List<QuestionOption> Choices { get; set; } = new();
>>>>>>> Stashed changes

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
            if (string.IsNullOrEmpty(FormId))
            {
                ModelState.AddModelError(nameof(FormId), "FormId is required.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Question.FormId = FormId;

<<<<<<< Updated upstream
=======
                // Add options for SingleChoice or LikertScale
                if (Question.Type == QuestionType.SingleChoice || Question.Type == QuestionType.LikertScale)
                {
                    if (Choices == null || Choices.Count == 0)
                    {
                        ModelState.AddModelError("Choices", "Choices or scale values are required for this question type.");
                        return Page();
                    }

                    Question.Options = Choices
                        .Where(choice => !string.IsNullOrWhiteSpace(choice.OptionText))
                        .ToList();

                    if (Question.Options.Count == 0)
                    {
                        ModelState.AddModelError("Choices", "At least one valid choice is required.");
                        return Page();
                    }
                }

>>>>>>> Stashed changes
                // Auto-increment question number
                var lastQuestionNumber = await _context.FormQuestions
                    .Where(q => q.FormId == FormId)
                    .OrderByDescending(q => q.QuestionNumber)
                    .Select(q => q.QuestionNumber)
                    .FirstOrDefaultAsync();

                Question.QuestionNumber = lastQuestionNumber + 1;

                // Add question to the database
                _context.FormQuestions.Add(Question);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the question. Please try again.");
                Console.WriteLine($"Error saving question: {ex.Message}");
                return Page();
            }

            return RedirectToPage("./Edit", new { id = FormId });
        }
    }
}
