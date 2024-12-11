using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize(Roles = "High-Auth,Study Admin")]
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
        public string FormQuestionId { get; set; }

        public FormQuestion Question { get; set; }

        [BindProperty]
        public List<FormQuestionOption> Options { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string questionId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(questionId))
            {
                Console.WriteLine("GET: FormId or QuestionId is missing.");
                return NotFound("FormId or QuestionId is missing.");
            }

            FormId = formId;
            FormQuestionId = questionId;

            Question = await _context.FormQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.FormQuestionId == questionId);

            if (Question == null)
            {
                Console.WriteLine("GET: Question not found.");
                return NotFound("Question not found.");
            }

            Options = Question.Options.OrderBy(o => o.OptionText).ToList();

            Console.WriteLine($"GET: Loaded Question: {Question.QuestionDescription}");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"POST: FormId: {FormId}, FormQuestionId: {FormQuestionId}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("POST: ModelState is invalid.");
                foreach (var error in ModelState)
                {
                    if (error.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"ModelState Error for {error.Key}: {error.Value.Errors.First().ErrorMessage}");
                    }
                }
                return Page();
            }

            var existingQuestion = await _context.FormQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.FormQuestionId == FormQuestionId);

            if (existingQuestion == null)
            {
                Console.WriteLine("POST: Question not found.");
                return NotFound("Question not found.");
            }

            var existingOptions = existingQuestion.Options.ToDictionary(o => o.OptionId);
            var submittedOptionIds = Options.Select(o => o.OptionId).ToHashSet();

            // Add or update options
            foreach (var option in Options)
            {
                // Check if the option ID already exists in the database
                var duplicateOption = await _context.FormQuestionOptions
                    .FirstOrDefaultAsync(o => o.OptionId == option.OptionId && o.FormQuestionId == existingQuestion.FormQuestionId);

                if (duplicateOption != null)
                {
                    // Update the existing option
                    duplicateOption.OptionText = option.OptionText;
                    duplicateOption.OptionValue = option.OptionValue;
                    _context.Entry(duplicateOption).State = EntityState.Modified;
                    Console.WriteLine($"Updated existing option: {duplicateOption.OptionId}, Text: {duplicateOption.OptionText}, Value: {duplicateOption.OptionValue}");
                }
                else
                {
                    // Add a new option if no duplicate is found
                    option.FormQuestionId = existingQuestion.FormQuestionId;
                    _context.FormQuestionOptions.Add(option);
                    Console.WriteLine($"Added new option: {option.OptionText}, Value: {option.OptionValue}");
                }
            }


            Console.WriteLine("POST: Saving changes...");
            try
            {
                var rowsAffected = await _context.SaveChangesAsync();
                Console.WriteLine($"POST: SaveChangesAsync completed. Rows affected: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST: Exception during SaveChangesAsync: {ex.Message}");
                throw;
            }

            return RedirectToPage("./Edit", new { id = FormId });
        }
    }
}
