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
using System.Security.Claims;
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
        public ParticipantSession ParticipantSession { get; set; }

        [BindProperty]
        public List<FormAnswer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(string formId, string participantId, string sessionId)
        {
            if (string.IsNullOrEmpty(formId) || string.IsNullOrEmpty(participantId) || string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("FormId, ParticipantId, and SessionId are required.");
            }

            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roles.Contains("Study Admin") && !roles.Contains("High-Auth") && !roles.Contains("Mid-Auth") && !roles.Contains("Low-Auth") && !roles.Contains("Researcher"))
            {
                return Forbid();
            }

            FormId = formId;
            ParticipantId = participantId;
            SessionId = sessionId;

            FormName = await _context.Forms
                .Where(f => f.FormId == formId)
                .Select(f => f.FormName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(FormName))
            {
                return NotFound("Form not found.");
            }

            ParticipantName = await _context.Participants
                .Where(p => p.ParticipantId == participantId)
                .Select(p => p.ParticipantFirstName + " " + p.ParticipantLastName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(ParticipantName))
            {
                return NotFound("Participant not found.");
            }

            Questions = await _context.FormQuestions
                .Where(q => q.FormId == formId)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            if (Questions == null || !Questions.Any())
            {
                return NotFound("No questions found for this form.");
            }

            ParticipantSession = await _context.ParticipantSessions
                .Include(ps => ps.Participant)
                .Include(ps => ps.Session)
                .FirstOrDefaultAsync(ps => ps.ParticipantId == participantId && ps.SessionId == sessionId);

            if (ParticipantSession == null)
            {
                return NotFound("Participant and session relationship not found.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Questions = await _context.FormQuestions
                .Where(q => q.FormId == FormId)
                .ToListAsync();

            if (Answers == null || !Answers.Any())
            {
                ModelState.AddModelError(string.Empty, "Answers are required for all questions.");
                return Page();
            }

            foreach (var answer in Answers)
            {
                var formQuestion = Questions.FirstOrDefault(q => q.FormQuestionId == answer.FormQuestionId);
                if (formQuestion == null)
                {
                    ModelState.AddModelError(string.Empty, $"Invalid question ID provided: {answer.FormQuestionId}");
                    return Page();
                }

                _context.FormAnswers.Add(new FormAnswer
                {
                    AnswerId = Guid.NewGuid().ToString(),
                    FormQuestionId = formQuestion.FormQuestionId,
                    FormId = FormId,
                    ParticipantId = ParticipantId,
                    SessionId = SessionId,
                    Answer = answer.Answer,
                    TimeStamp = DateTime.UtcNow,
                    FormQuestion = formQuestion,
                    ParticipantSession = ParticipantSession
                });
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Page();
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
