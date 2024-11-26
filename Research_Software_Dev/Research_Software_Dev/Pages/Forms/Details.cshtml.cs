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
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") && !roles.Contains("Mid-Auth"))
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

            return Page();
        }
    }
}
