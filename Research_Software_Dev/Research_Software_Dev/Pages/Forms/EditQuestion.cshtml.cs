using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IActionResult> OnGetAsync(string formId, string questionId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(questionId))
            {
                return NotFound("FormId or QuestionId is missing.");
            }

            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            Question = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormId == formId && q.FormQuestionId == questionId);

            if (Question == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingQuestion = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormQuestionId == Question.FormQuestionId);

            if (existingQuestion == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Update fields
            existingQuestion.QuestionDescription = Question.QuestionDescription;
            existingQuestion.Type = Question.Type;
            existingQuestion.OptionsJson = Question.OptionsJson;
            existingQuestion.Category = Question.Category;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.FormQuestions.AnyAsync(q => q.FormQuestionId == Question.FormQuestionId))
                {
                    return RedirectToPage("/NotFound");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Edit", new { id = Question.FormId });
        }
    }
}
