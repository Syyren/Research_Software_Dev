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
            Form = await _context.Forms
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(f => f.FormId == id);

            if (Form == null)
            {
                return RedirectToPage("/NotFound");
            }

            Questions = Form.Questions.OrderBy(q => q.QuestionNumber).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var existingForm = await _context.Forms
                .Include(f => f.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(f => f.FormId == Form.FormId);

            if (existingForm == null) return RedirectToPage("/NotFound");

            existingForm.FormName = Form.FormName;

            foreach (var question in Questions)
            {
                var existingQuestion = existingForm.Questions
                    .FirstOrDefault(q => q.FormQuestionId == question.FormQuestionId);

                if (existingQuestion != null)
                {
                    existingQuestion.QuestionDescription = question.QuestionDescription;
                    existingQuestion.Type = question.Type;
                    existingQuestion.QuestionNumber = question.QuestionNumber;
                    existingQuestion.Category = question.Category;
                    existingQuestion.Options = question.Options ?? new List<QuestionOption>();
                }
                else
                {
                    question.FormId = existingForm.FormId;
                    _context.FormQuestions.Add(question);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
