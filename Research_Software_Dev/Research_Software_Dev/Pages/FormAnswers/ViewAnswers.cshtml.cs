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
    public class ViewAnswersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewAnswersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public string SessionId { get; set; }

        public List<FormAnswer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string participantId, string sessionId)
        {
            FormId = formId;
            ParticipantId = participantId;
            SessionId = sessionId;

            // Fetches answers from the database using composite key
            Answers = await _context.FormAnswers
                .Include(a => a.FormQuestion) // Includes question data
                .Where(a => a.FormId == formId && a.ParticipantId == participantId && a.SessionId == sessionId)
                .ToListAsync();

            if (!Answers.Any())
            {
                return NotFound("No answers found for the specified form, participant, and session.");
            }

            return Page();
        }
    }
}