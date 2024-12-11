using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Data
{
    public class SessionCategoryScoreModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SessionCategoryScoreModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SelectedParticipantId { get; set; }

        [BindProperty]
        public string SelectedSessionId { get; set; }

        public List<Participant> AvailableParticipants { get; set; } = new List<Participant>();
        public List<Session> AvailableSessions { get; set; } = new List<Session>();
        public string ChartDataJson { get; private set; }

        public async Task OnGetAsync(string studyId, string participantId, string sessionId)
        {
            if (string.IsNullOrEmpty(studyId))
            {
                Console.WriteLine("Study ID is required.");
                return;
            }

            Console.WriteLine($"Fetching data for Study ID: {studyId}");

            // Get the current researcher's ID
            var researcherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(researcherId))
            {
                Console.WriteLine("Researcher ID is required.");
                return;
            }

            // Fetch participant IDs for the specified study
            var participantIdsInStudy = await _context.ParticipantStudies
                .Where(ps => ps.StudyId == studyId)
                .Select(ps => ps.ParticipantId)
                .Distinct()
                .ToListAsync();

            // Fetch participants linked to the study
            AvailableParticipants = await _context.Participants
                .Where(p => participantIdsInStudy.Contains(p.ParticipantId))
                .OrderBy(p => p.ParticipantFirstName)
                .ToListAsync();

            Console.WriteLine($"Available Participants: {AvailableParticipants.Count}");

            // Fetch session IDs linked to these participants
            var sessionIdsForParticipants = await _context.ParticipantSessions
                .Where(ps => participantIdsInStudy.Contains(ps.ParticipantId))
                .Select(ps => ps.SessionId)
                .Distinct()
                .ToListAsync();

            // Fetch sessions linked to the study and participants
            AvailableSessions = await _context.Sessions
                .Where(s => s.StudyId == studyId && sessionIdsForParticipants.Contains(s.SessionId))
                .OrderBy(s => s.Date)
                .ToListAsync();

            Console.WriteLine($"Available Sessions: {AvailableSessions.Count}");

            // Process the chart generation if participant and session are selected
            if (!string.IsNullOrEmpty(participantId) && !string.IsNullOrEmpty(sessionId))
            {
                SelectedParticipantId = participantId;
                SelectedSessionId = sessionId;

                Console.WriteLine($"Generating chart for Participant: {participantId}, Session: {sessionId}");

                var answers = await _context.FormAnswers
                    .Where(a => a.ParticipantId == participantId && a.SessionId == sessionId)
                    .Include(a => a.FormQuestion)
                    .ToListAsync();

                Console.WriteLine($"Fetched {answers.Count} answers");

                // Load option values for answers with selected options
                var optionValues = await _context.FormQuestionOptions
                    .Where(o => answers.Select(a => a.SelectedOption).Contains(o.OptionId))
                    .ToDictionaryAsync(o => o.OptionId, o => o.OptionValue);

                foreach (var answer in answers)
                {
                    if (answer.SelectedOption != null && optionValues.TryGetValue(answer.SelectedOption, out var value))
                    {
                        Console.WriteLine($"Answer: ParticipantId={answer.ParticipantId}, SessionId={answer.SessionId}, Category={answer.FormQuestion.Category}, OptionValue={value}");
                    }
                }

                var groupedData = answers
                    .Where(a => a.SelectedOption != null && optionValues.ContainsKey(a.SelectedOption))
                    .GroupBy(a => a.FormQuestion.Category)
                    .Select(g => new
                    {
                        Category = g.Key,
                        Score = g.Sum(a => optionValues[a.SelectedOption])
                    })
                    .ToList();

                Console.WriteLine($"Grouped Data: {JsonConvert.SerializeObject(groupedData)}");

                ChartDataJson = JsonConvert.SerializeObject(new
                {
                    labels = groupedData.Select(g => g.Category).ToList(),
                    scores = groupedData.Select(g => g.Score).ToList(),
                    colors = groupedData.Select(_ => GetRandomColor()).ToList()
                });

                Console.WriteLine($"ChartDataJson: {ChartDataJson}");
            }
            else
            {
                Console.WriteLine("No participant or session selected.");
            }
        }

        private string GetRandomColor()
        {
            var random = new Random();
            return $"rgba({random.Next(0, 255)}, {random.Next(0, 255)}, {random.Next(0, 255)}, 1)";
        }
    }
}
