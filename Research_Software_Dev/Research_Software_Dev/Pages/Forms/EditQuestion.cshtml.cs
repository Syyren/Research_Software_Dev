using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class EditQuestionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditQuestionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FormQuestion Question { get; set; }

        [BindProperty]
        public List<QuestionOption> Options { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string formId, string questionId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(questionId))
            {
                return NotFound("FormId or QuestionId is missing.");
            }

            Question = await _context.FormQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.FormId == formId && q.FormQuestionId == questionId);

            if (Question == null)
            {
                return RedirectToPage("/NotFound");
            }

            Options = Question.Options;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingQuestion = await _context.FormQuestions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.FormQuestionId == Question.FormQuestionId);

            if (existingQuestion == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Update fields
            existingQuestion.QuestionDescription = Question.QuestionDescription;
            existingQuestion.Type = Question.Type;
            existingQuestion.Category = Question.Category;

            // Update options
            existingQuestion.Options.Clear();
            existingQuestion.Options.AddRange(Options);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = Question.FormId });
        }
    }
}
