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
    public class SelectFormModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SelectFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ParticipantSessionId { get; set; }

        [BindProperty]
        public string FormId { get; set; }

        public List<Form> Forms { get; set; }

        public async Task<IActionResult> OnGetAsync(int participantSessionId)
        {
            ParticipantSessionId = participantSessionId;

            // Loads available forms from the database
            Forms = await _context.Forms.ToListAsync();

            if (!Forms.Any())
            {
                return NotFound("No forms available.");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./SubmitAnswers", new { formId = FormId, participantSessionId = ParticipantSessionId });
        }
    }
}