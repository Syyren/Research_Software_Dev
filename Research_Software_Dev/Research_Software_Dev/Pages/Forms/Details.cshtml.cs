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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Form Form { get; set; }
        public List<FormQuestion> Questions { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Check user roles
            var roles = User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") && !roles.Contains("Mid-Auth") && !roles.Contains("Low-Auth") && !roles.Contains("Researcher"))
            {
                return Forbid();
            }

            // Load the form
            Form = await _context.Forms.FirstOrDefaultAsync(f => f.FormId == id);
            if (Form == null)
            {
                return RedirectToPage("/NotFound");
            }

            // Load questions with their options
            Questions = await _context.FormQuestions
                .Include(q => q.Options)
                .Where(q => q.FormId == id)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            // Log options for each question
            foreach (var question in Questions)
            {
                if (question.Options != null)
                {
                    foreach (var option in question.Options)
                    {
                        System.Console.WriteLine($"Question {question.QuestionNumber}, Option: {option.OptionText}, Value: {option.OptionValue ?? 0}");
                    }
                }
            }

            return Page();
        }
    }
}
