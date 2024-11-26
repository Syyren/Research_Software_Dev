using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Forms
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Form Form { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin"))
            {
                return Forbid();
            }

            Form = await _context.Forms.FindAsync(id);

            if (Form == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin"))
            {
                return Forbid();
            }

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
