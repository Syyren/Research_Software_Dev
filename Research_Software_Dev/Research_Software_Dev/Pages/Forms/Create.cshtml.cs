using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
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

        public void OnGet()
        {
            // Pre-generate FormId
            Form = new Form
            {
                FormId = Guid.NewGuid().ToString()
            };
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

            Console.WriteLine($"FormId: {Form.FormId}");
            Console.WriteLine($"FormName: {Form.FormName}");
            Console.WriteLine($"Questions: {string.Join(", ", Questions)}");

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
