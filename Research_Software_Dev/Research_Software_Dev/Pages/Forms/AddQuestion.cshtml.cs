using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
        public FormQuestion Question { get; set; }

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public List<string> Choices { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return NotFound("FormId is missing.");
            }

            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") && !roles.Contains("Mid-Auth") && !roles.Contains("Researcher"))
            {
                return Forbid();
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

                // Process Choices or Scale for SingleChoice or LikertScale
                if (Question.Type == QuestionType.SingleChoice || Question.Type == QuestionType.LikertScale)
                {
                    if (Choices == null || Choices.Count == 0)
                    {
                        ModelState.AddModelError("Choices", "Choices or scale values are required for this question type.");
                        return Page();
                    }

                    // Remove empty or duplicate choices and serialize
                    var formattedChoices = Choices
                        .Where(choice => !string.IsNullOrWhiteSpace(choice))
                        .Select(choice => choice.Trim())
                        .Distinct()
                        .ToList();

                    if (formattedChoices.Count == 0)
                    {
                        ModelState.AddModelError("Choices", "At least one valid choice or scale value is required.");
                        return Page();
                    }

                    Question.OptionsJson = JsonSerializer.Serialize(formattedChoices);
                }

                // Auto-increment question number
                var lastQuestionNumber = await _context.FormQuestions
                    .Where(q => q.FormId == FormId)
                    .OrderByDescending(q => q.QuestionNumber)
                    .Select(q => q.QuestionNumber)
                    .FirstOrDefaultAsync();

                Question.QuestionNumber = lastQuestionNumber + 1;

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
