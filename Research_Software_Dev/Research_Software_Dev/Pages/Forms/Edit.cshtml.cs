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
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Form Form { get; set; }

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public List<FormQuestion> Questions { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Form = await _context.Forms
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(f => f.FormId == id);

            if (Form == null)
            {
                return NotFound("Form not found.");
            }

            Questions = Form.Questions.OrderBy(q => q.QuestionNumber).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
<<<<<<< Updated upstream
            Console.WriteLine($"POST: Editing Form - FormId: {Form.FormId}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    if (error.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"ModelState Error for {error.Key}: {error.Value.Errors.First().ErrorMessage}");
                    }
                }
                return Page();
            }

            var existingForm = await _context.Forms
                .Include(f => f.Questions)
                .FirstOrDefaultAsync(f => f.FormId == Form.FormId);

            if (existingForm == null)
            {
                return NotFound("Form not found.");
            }
=======
            if (!ModelState.IsValid) return Page();

            var existingForm = await _context.Forms
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(f => f.FormId == Form.FormId);

            if (existingForm == null) return RedirectToPage("/NotFound");
>>>>>>> Stashed changes

            existingForm.FormName = Form.FormName;

<<<<<<< Updated upstream
            Console.WriteLine($"POST: Processing Questions for FormId: {Form.FormId}");
            var existingQuestions = existingForm.Questions.ToDictionary(q => q.FormQuestionId, q => q);
            var submittedQuestionIds = Questions.Select(q => q.FormQuestionId).ToHashSet();

            // Add or update questions
            foreach (var question in Questions)
            {
                if (existingQuestions.TryGetValue(question.FormQuestionId, out var existingQuestion))
=======
            foreach (var question in Questions)
            {
                var existingQuestion = existingForm.Questions
                    .FirstOrDefault(q => q.FormQuestionId == question.FormQuestionId);

                if (existingQuestion != null)
>>>>>>> Stashed changes
                {
                    existingQuestion.QuestionDescription = question.QuestionDescription;
<<<<<<< Updated upstream
=======
                    existingQuestion.Type = question.Type;
>>>>>>> Stashed changes
                    existingQuestion.QuestionNumber = question.QuestionNumber;
                    existingQuestion.Type = question.Type;
                    existingQuestion.Category = question.Category;
<<<<<<< Updated upstream
                    Console.WriteLine($"Updated Question: {existingQuestion.FormQuestionId} - {existingQuestion.QuestionDescription}");
                }
                else
                {
                    // Add new question
                    question.FormId = Form.FormId;
=======
                    existingQuestion.Options = question.Options ?? new List<QuestionOption>();
                }
                else
                {
                    question.FormId = existingForm.FormId;
>>>>>>> Stashed changes
                    _context.FormQuestions.Add(question);
                    Console.WriteLine($"Added New Question: {question.FormQuestionId} - {question.QuestionDescription}");
                }
            }

<<<<<<< Updated upstream
            // Remove questions not submitted
            foreach (var questionId in existingQuestions.Keys.Except(submittedQuestionIds))
            {
                var questionToRemove = existingQuestions[questionId];
                _context.FormQuestions.Remove(questionToRemove);
                Console.WriteLine($"Removed Question: {questionToRemove.FormQuestionId} - {questionToRemove.QuestionDescription}");
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

=======
            await _context.SaveChangesAsync();
>>>>>>> Stashed changes
            return RedirectToPage("./Index");
        }

    }
}
