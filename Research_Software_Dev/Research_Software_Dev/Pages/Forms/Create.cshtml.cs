using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Form Form { get; set; }

        [BindProperty]
        public List<string> Questions { get; set; } = new();

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth"))
            {
                return Forbid();
            }

            Form = new Form
            {
                FormId = Guid.NewGuid().ToString()
            };

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
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            _context.Forms.Add(Form);
            await _context.SaveChangesAsync();

            foreach (var questionText in Questions)
            {
                _context.FormQuestions.Add(new FormQuestion
                {
                    FormQuestionId = Guid.NewGuid().ToString(),
                    QuestionDescription = questionText,
                    FormId = Form.FormId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
