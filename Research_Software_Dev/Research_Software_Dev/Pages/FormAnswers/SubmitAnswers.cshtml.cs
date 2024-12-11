using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
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
                .Include(q => q.Options)
                .Where(q => q.FormId == formId)
                .Include(q => q.Options)
                .OrderBy(q => q.QuestionNumber)
                .ToListAsync();

            if (!Questions.Any())
                return NotFound("No questions found for this form.");

<<<<<<< Updated upstream
            ParticipantSession = await _context.ParticipantSessions
                .FirstOrDefaultAsync(ps => ps.ParticipantId == participantId && ps.SessionId == sessionId);

            if (ParticipantSession == null)
                return NotFound("Participant and session relationship not found.");

=======
>>>>>>> Stashed changes
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        { 

            Console.WriteLine("POST: Logging submitted answers...");
            foreach (var answer in Answers)
            {
                Console.WriteLine($"AnswerId: {answer.AnswerId}");
                Console.WriteLine($"FormQuestionId: {answer.FormQuestionId}");
                Console.WriteLine($"ParticipantId: {answer.ParticipantId}");
                Console.WriteLine($"SessionId: {answer.SessionId}");
                Console.WriteLine($"TextAnswer: {answer.TextAnswer}");
                Console.WriteLine($"SelectedOption: {answer.SelectedOption}");
                Console.WriteLine($"TimeStamp: {answer.TimeStamp}");
            }

            if (!ModelState.IsValid || Answers == null || !Answers.Any())
            {
                Console.WriteLine("POST: ModelState is invalid or no answers received.");
                foreach (var error in ModelState)
                {
                    if (error.Value.Errors.Any())
                    {
                        Console.WriteLine($"ModelState Error for {error.Key}: {error.Value.Errors.First().ErrorMessage}");
                    }
                }
                return Page();
            }

            foreach (var answer in Answers)
            {
<<<<<<< Updated upstream
                if (!string.IsNullOrEmpty(answer.SelectedOption))
                {
                    var selectedOption = await _context.FormQuestionOptions
                        .FirstOrDefaultAsync(o => o.OptionId == answer.SelectedOption);

                    if (selectedOption != null)
                    {
                        answer.TextAnswer = selectedOption.OptionText;
                    }
=======
                var question = Questions.FirstOrDefault(q => q.FormQuestionId == answer.FormQuestionId);
                if (question != null)
                {
                    var option = question.Options.FirstOrDefault(o => o.OptionText == answer.TextAnswer);
                    answer.ChoiceValue = option?.OptionValue;
                    answer.TimeStamp = DateTime.UtcNow;
>>>>>>> Stashed changes
                }

                _context.FormAnswers.Add(answer);
            }

            try
            {
                var rowsAffected = await _context.SaveChangesAsync();
                Console.WriteLine($"POST: SaveChangesAsync completed. Rows affected: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST: Exception during SaveChangesAsync: {ex.Message}");
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
