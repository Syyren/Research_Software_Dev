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
        public string FormId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public string SessionId { get; set; }

        public List<Form> Forms { get; set; }

        public async Task<IActionResult> OnGetAsync(string participantId, string sessionId)
        {
            ParticipantId = participantId;
            SessionId = sessionId;

            Forms = await _context.Forms.OrderBy(f => f.FormName).ToListAsync();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(FormId))
            {
                ModelState.AddModelError(string.Empty, "Please select a form.");
                return Page();
            }

            return RedirectToPage("./SubmitAnswers", new { formId = FormId, participantId = ParticipantId, sessionId = SessionId });
        }
    }
}
