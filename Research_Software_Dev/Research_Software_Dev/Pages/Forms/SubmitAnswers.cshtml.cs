using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System;
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
        public string FormId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public string SessionId { get; set; }

        public List<FormQuestion> Questions { get; set; }

        [BindProperty]
        public List<FormAnswer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string participantId, string sessionId)
        {
            FormId = formId;
            ParticipantId = participantId;
            SessionId = sessionId;

            // Loads questions for the form
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
            // Reloads Questions to ensure they're initialized
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

            // Validates and saves answers
            foreach (var answer in Answers)
            {
                if (string.IsNullOrEmpty(answer.QuestionId) || string.IsNullOrEmpty(answer.Answer))
                {
                    ModelState.AddModelError(string.Empty, "All questions must be answered.");
                    return Page();
                }

                // Assigns the composite key and other fields
                answer.FormId = FormId;
                answer.ParticipantId = ParticipantId;
                answer.SessionId = SessionId;
                answer.TimeStamp = DateTime.UtcNow;

                _context.FormAnswers.Add(answer);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./ViewAnswers", new { formId = FormId, participantId = ParticipantId, sessionId = SessionId });
        }
    }
}