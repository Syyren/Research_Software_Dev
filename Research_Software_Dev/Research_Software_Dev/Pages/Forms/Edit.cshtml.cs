using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
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
        public List<FormQuestion> Questions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Fetch form with related questions
            Form = await _context.Forms.Include(f => f.Questions).FirstOrDefaultAsync(f => f.FormId == id);

            if (Form == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Order questions by number and populate Questions property
            Questions = Form.Questions.OrderBy(q => q.QuestionNumber).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the model
            if (!ModelState.IsValid)
                return Page();

            // Fetch the existing form
            var existingForm = await _context.Forms.Include(f => f.Questions).FirstOrDefaultAsync(f => f.FormId == Form.FormId);

            if (existingForm == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Update form name
            existingForm.FormName = Form.FormName;

            // Update or add questions
            foreach (var question in Questions)
            {
                var existingQuestion = existingForm.Questions.FirstOrDefault(q => q.FormQuestionId == question.FormQuestionId);

                if (existingQuestion != null)
                {
                    // Update existing question
                    existingQuestion.QuestionDescription = question.QuestionDescription;
                    existingQuestion.Type = question.Type;
                    existingQuestion.OptionsJson = question.OptionsJson;
                    existingQuestion.QuestionNumber = question.QuestionNumber;
                    existingQuestion.Category = question.Category;
                }
                else
                {
                    // Add new question
                    question.FormId = existingForm.FormId;
                    _context.FormQuestions.Add(question);
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
