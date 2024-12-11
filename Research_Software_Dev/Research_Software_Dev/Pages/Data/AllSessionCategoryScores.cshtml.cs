using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Research_Software_Dev.Pages.Data
{
    [Authorize(Roles = "Mid-Auth,High-Auth,Study Admin")]
    public class AllSessionCategoryScoresModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AllSessionCategoryScoresModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SelectedSessionId { get; set; }

        public List<Session> AvailableSessions { get; set; } = new List<Session>();
        public string ChartDataJson { get; private set; }

        public async Task OnGetAsync(string sessionId)
        {
            Console.WriteLine("Fetching data for All Session Category Scores...");

            // Get the current researcher's ID
            var researcherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (researcherId == null)
            {
                Console.WriteLine("ERROR: Researcher ID is null.");
                return;
            }

            // Fetch studies associated with the researcher
            var researcherStudyIds = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Select(rs => rs.StudyId)
                .ToListAsync();

            Console.WriteLine($"Researcher Study IDs: {string.Join(", ", researcherStudyIds)}");

            // Fetch session IDs linked to these studies
            var sessionIdsInStudies = await _context.Sessions
                .Where(s => researcherStudyIds.Contains(s.StudyId))
                .Select(s => s.SessionId)
                .Distinct()
                .ToListAsync();

            // Load available sessions scoped to the researcher
            AvailableSessions = await _context.Sessions
                .Where(s => sessionIdsInStudies.Contains(s.SessionId))
                .OrderBy(s => s.Date)
                .ToListAsync();

            Console.WriteLine("Filtered Available Sessions:");
            foreach (var session in AvailableSessions)
            {
                Console.WriteLine($"- SessionId: {session.SessionId}, Date: {session.Date}");
            }

            if (!string.IsNullOrEmpty(sessionId))
            {
                SelectedSessionId = sessionId;

                Console.WriteLine($"Generating chart for SessionId: {sessionId}");

                // Fetch answers for the selected session
                var filteredAnswers = await _context.FormAnswers
                    .Where(a => a.SessionId == sessionId)
                    .Include(a => a.FormQuestion)
                    .ToListAsync();

                Console.WriteLine($"FilteredAnswers Count: {filteredAnswers.Count}");
                foreach (var answer in filteredAnswers)
                {
                    Console.WriteLine($"Answer: ParticipantId={answer.ParticipantId}, Category={answer.FormQuestion.Category}, SelectedOption={answer.SelectedOption}");
                }

                // Load option values for answers with selected options
                var optionValues = await _context.FormQuestionOptions
                    .Where(o => filteredAnswers.Select(a => a.SelectedOption).Contains(o.OptionId))
                    .ToDictionaryAsync(o => o.OptionId, o => o.OptionValue);

                // Retrieve participant names
                var participantIds = filteredAnswers.Select(a => a.ParticipantId).Distinct().ToList();
                var participantDetails = await _context.Participants
                    .Where(p => participantIds.Contains(p.ParticipantId))
                    .ToDictionaryAsync(p => p.ParticipantId, p => p.ParticipantFirstName + " " + p.ParticipantLastName);

                // Group data by category and participant
                var groupedData = filteredAnswers
                    .Where(a => !string.IsNullOrEmpty(a.SelectedOption) && optionValues.ContainsKey(a.SelectedOption))
                    .GroupBy(a => new { a.FormQuestion.Category, a.ParticipantId })
                    .Select(g => new
                    {
                        g.Key.Category,
                        ParticipantName = participantDetails[g.Key.ParticipantId], // Map ParticipantId to Name
                        TotalScore = g.Sum(a => optionValues[a.SelectedOption]) // Use OptionValue
                    })
                    .ToList();

                Console.WriteLine($"GroupedData Count: {groupedData.Count}");
                foreach (var group in groupedData)
                {
                    Console.WriteLine($"Category: {group.Category}, ParticipantName: {group.ParticipantName}, TotalScore: {group.TotalScore}");
                }

                // Prepare data for Chart.js
                var participants = groupedData.Select(g => g.ParticipantName).Distinct().ToList();
                var categories = groupedData.Select(g => g.Category).Distinct().ToList();

                var scoresByCategory = categories.Select(category =>
                    participants.Select(participant =>
                        groupedData
                            .Where(g => g.Category == category && g.ParticipantName == participant)
                            .Select(g => g.TotalScore)
                            .FirstOrDefault()
                    ).ToList()
                ).ToList();

                ChartDataJson = JsonConvert.SerializeObject(new
                {
                    participants,
                    categories,
                    scores = scoresByCategory,
                    colors = categories.Select(_ => GetRandomColor()).ToList()
                });

                Console.WriteLine($"ChartDataJson: {ChartDataJson}");
            }
            else
            {
                Console.WriteLine("No session selected.");
            }
        }

        private string GetRandomColor()
        {
            var random = new Random();
            return $"rgba({random.Next(0, 255)}, {random.Next(0, 255)}, {random.Next(0, 255)}, 1)";
        }
    }
}
