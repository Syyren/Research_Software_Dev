using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            Form = await _context.Forms.FindAsync(id);
            if (Form == null)
            {
                return NotFound();
            }

            Questions = await _context.FormQuestions
                .Where(q => q.FormId == id)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            foreach (var question in Questions)
            {
                question.FormId = id;
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

            var existingForm = await _context.Forms
                .Include(f => f.Questions)
                .FirstOrDefaultAsync(f => f.FormId == Form.FormId);

            if (existingForm == null)
            {
                return NotFound();
            }

            existingForm.FormName = Form.FormName;

            foreach (var question in Questions)
            {
                question.FormId = existingForm.FormId;

                if (string.IsNullOrEmpty(question.FormQuestionId))
                {
                    _context.FormQuestions.Add(question);
                }
                else
                {
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
