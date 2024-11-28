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
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            FormId = formId;
            QuestionId = questionId;

            var question = await _context.FormQuestions
                .FirstOrDefaultAsync(q => q.FormQuestionId == questionId && q.FormId == formId);

            if (question == null)
            {
                return RedirectToPage("/NotFound");
            }

            QuestionDescription = question.QuestionDescription;

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
