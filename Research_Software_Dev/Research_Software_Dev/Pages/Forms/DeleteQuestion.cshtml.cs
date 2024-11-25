using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;

namespace Research_Software_Dev.Pages.Forms
{
    public class DeleteQuestionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteQuestionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string QuestionId { get; set; }

        [BindProperty]
        public string FormId { get; set; }

        public string QuestionDescription { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string questionId)
        {
            FormId = formId;
            QuestionId = questionId;

            var question = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormQuestionId == questionId && q.FormId == formId);

            if (question == null)
            {
                return NotFound();
            }

            QuestionDescription = question.QuestionDescription;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var question = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormQuestionId == QuestionId && q.FormId == FormId);

            if (question != null)
            {
                _context.FormQuestions.Remove(question);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Edit", new { id = FormId });
        }
    }
}
