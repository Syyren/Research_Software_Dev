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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Form = await _context.Forms.FindAsync(id);
            if (Form == null)
            {
                return NotFound();
            }

            Questions = await _context.FormQuestions
                .Where(q => q.FormId == id)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            // Load the form from the database
            var existingForm = await _context.Forms.FindAsync(Form.FormId);

            if (existingForm == null)
            {
                return NotFound();
            }

            // Update form properties
            existingForm.FormName = Form.FormName;

            var questions = existingForm.Questions
            .OrderBy(q => q.QuestionNumber)
            .ToList();

            for (int i = 0; i < questions.Count; i++)
            {
                questions[i].QuestionNumber = i + 1;
                _context.Entry(questions[i]).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Forms.Any(f => f.FormId == Form.FormId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redirect to the Index page or another appropriate page
            return RedirectToPage("./Index");
        }
    }
}