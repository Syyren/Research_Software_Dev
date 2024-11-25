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
            Form = await _context.Forms.FindAsync(id);
            if (Form == null)
            {
                return NotFound();
            }

            Questions = await _context.FormQuestions
                .Where(q => q.FormId == id)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            // Ensure FormId is assigned to all questions
            foreach (var question in Questions)
            {
                question.FormId = id;
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

            var existingForm = await _context.Forms
                .Include(f => f.Questions)
                .FirstOrDefaultAsync(f => f.FormId == Form.FormId);

            if (existingForm == null)
            {
                return NotFound();
            }

            // Update the form's name
            existingForm.FormName = Form.FormName;

            // Update or add questions
            foreach (var question in Questions)
            {
                question.FormId = existingForm.FormId;

                if (string.IsNullOrEmpty(question.FormQuestionId))
                {
                    // Add new question
                    _context.FormQuestions.Add(question);
                }
                else
                {
                    // Update existing question
                    var existingQuestion = existingForm.Questions
                        .FirstOrDefault(q => q.FormQuestionId == question.FormQuestionId);

                    if (existingQuestion != null)
                    {
                        existingQuestion.QuestionDescription = question.QuestionDescription;
                        existingQuestion.QuestionNumber = question.QuestionNumber;
                        _context.Entry(existingQuestion).State = EntityState.Modified;
                    }
                }
            }

            // Renumber questions sequentially
            var reorderedQuestions = Questions.OrderBy(q => q.QuestionNumber).ToList();
            for (int i = 0; i < reorderedQuestions.Count; i++)
            {
                reorderedQuestions[i].QuestionNumber = i + 1;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
