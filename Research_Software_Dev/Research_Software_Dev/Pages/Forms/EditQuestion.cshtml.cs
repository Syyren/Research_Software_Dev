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
        public string FormId { get; set; }

        [BindProperty]
        public FormQuestion Question { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string questionId)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Question = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormId == formId && q.FormQuestionId == questionId);

            if (Question == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingQuestion = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormQuestionId == Question.FormQuestionId);

            if (existingQuestion == null)
            {
                return NotFound();
            }

            existingQuestion.QuestionDescription = Question.QuestionDescription;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FormQuestions.Any(q => q.FormQuestionId == Question.FormQuestionId))
                {
                    return NotFound();
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
