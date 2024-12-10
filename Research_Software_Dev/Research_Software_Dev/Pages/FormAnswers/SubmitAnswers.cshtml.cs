using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class SubmitAnswersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SubmitAnswersModel(ApplicationDbContext context)
        {
            _context = context;
            Questions = new List<FormQuestion>();
        }

        [BindProperty]
        public string FormId { get; set; }

        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public string SessionId { get; set; }

        public string ParticipantName { get; set; }
        public string FormName { get; set; }
        public List<FormQuestion> Questions { get; set; }

        [BindProperty]
        public List<FormAnswer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string participantId, string sessionId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(participantId) || string.IsNullOrEmpty(sessionId))
                return BadRequest("FormId, ParticipantId, and SessionId are required.");

            FormId = formId;
            ParticipantId = participantId;
            SessionId = sessionId;

            FormName = await _context.Forms
                .Where(f => f.FormId == formId)
                .Select(f => f.FormName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(FormName))
                return NotFound("Form not found.");

            ParticipantName = await _context.Participants
                .Where(p => p.ParticipantId == participantId)
                .Select(p => p.ParticipantFirstName + " " + p.ParticipantLastName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(ParticipantName))
                return NotFound("Participant not found.");

            Questions = await _context.FormQuestions
                .Where(q => q.FormId == formId)
                .Include(q => q.Options)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            if (Questions == null || !Questions.Any())
                return NotFound("No questions found for this form.");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Answers == null || !Answers.Any())
            {
                ModelState.AddModelError(string.Empty, "You must answer all questions.");
                return Page();
            }

            foreach (var answer in Answers)
            {
                var question = Questions.FirstOrDefault(q => q.FormQuestionId == answer.FormQuestionId);
                if (question != null)
                {
                    var option = question.Options.FirstOrDefault(o => o.OptionText == answer.TextAnswer);
                    answer.ChoiceValue = option?.OptionValue;
                    answer.TimeStamp = DateTime.UtcNow;
                }

                _context.FormAnswers.Add(answer);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "There was an error saving your answers. Please try again.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
