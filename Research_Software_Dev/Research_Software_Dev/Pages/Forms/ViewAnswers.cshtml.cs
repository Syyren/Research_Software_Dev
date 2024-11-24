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
        public int FormId { get; set; }

        [BindProperty]
        public int ParticipantSessionId { get; set; }

        public List<FormAnswer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(int formId, int participantSessionId)
        {
            FormId = formId;
            ParticipantSessionId = participantSessionId;

            // Fetches answers from the database
            Answers = await _context.FormAnswers
                .Include(a => a.Question) // Includes question data
                .Where(a => a.FormId == formId && a.ParticipantSessionId == participantSessionId)
                .ToListAsync();

            if (!Answers.Any())
            {
                return NotFound("No answers found for the specified form and participant session.");
            }

            return Page();
        }
    }
}