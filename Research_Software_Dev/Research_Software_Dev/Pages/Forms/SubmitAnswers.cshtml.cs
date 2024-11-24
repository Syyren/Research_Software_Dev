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
    public class SubmitAnswersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SubmitAnswersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int FormId { get; set; }

        [BindProperty]
        public int ParticipantSessionId { get; set; }

        public List<FormQuestion> Questions { get; set; }

        [BindProperty]
        public List<FormAnswer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(int formId, int participantSessionId)
        {
            FormId = formId;
            ParticipantSessionId = participantSessionId;

            // Load questions for the form
            Questions = await _context.FormQuestions
                .Where(q => q.FormId == formId)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            if (!Questions.Any())
            {
                return NotFound("No questions found for this form.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reload Questions to ensure it's initialized
            Questions = await _context.FormQuestions
                .Where(q => q.FormId == FormId)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            if (Questions == null || !Questions.Any())
            {
                ModelState.AddModelError(string.Empty, "No questions found for this form.");
                return Page();
            }

            if (Answers == null || !Answers.Any())
            {
                ModelState.AddModelError(string.Empty, "No answers were provided.");
                return Page();
            }

            // Validate and save answers
            foreach (var answer in Answers)
            {
                if (string.IsNullOrEmpty(answer.QuestionId) || string.IsNullOrEmpty(answer.Answer))
                {
                    ModelState.AddModelError(string.Empty, "All questions must be answered.");
                    return Page();
                }

                // Assign participant session ID and timestamp
                answer.FormId = FormId;
                answer.ParticipantSessionId = ParticipantSessionId;
                answer.TimeStamp = DateTime.UtcNow;

                _context.FormAnswers.Add(answer);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./ViewAnswers", new { formId = FormId, participantSessionId = ParticipantSessionId });
        }
    }
}