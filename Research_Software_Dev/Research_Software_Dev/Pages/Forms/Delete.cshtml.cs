using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Form Form { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Form = await _context.Forms.FindAsync(id);

            if (Form == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var form = await _context.Forms.FindAsync(id);

            if (form != null)
            {
                var questions = _context.FormQuestions
                    .Where(q => q.FormId == form.FormId);

                _context.FormQuestions.RemoveRange(questions);
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}