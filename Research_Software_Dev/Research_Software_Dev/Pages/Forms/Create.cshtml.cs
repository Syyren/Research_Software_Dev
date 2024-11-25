using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Forms.Add(Form);
            await _context.SaveChangesAsync();
            Form.FormId = Guid.NewGuid().ToString();

            foreach (var questionText in Questions)
            {
                _context.FormQuestions.Add(new FormQuestion
                {
                    QuestionDescription = questionText,
                    FormId = Form.FormId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}