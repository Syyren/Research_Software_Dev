using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    [Authorize(Roles = "Mid-Auth,High-Auth,Study Admin")]
    public class CategoryTrendModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CategoryTrendModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string StudyId { get; set; }

        [BindProperty]
        public List<string> SelectedParticipants { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        public SelectList Studies { get; set; } = new SelectList(new List<SelectListItem>());
        public List<Participant> AvailableParticipants { get; set; } = new List<Participant>();
        public List<Session> AvailableSessions { get; set; } = new List<Session>();
        public string ChartDataJson { get; private set; }

        public async Task OnGetAsync(string studyId, DateTime? startDate, DateTime? endDate, List<string> participants)
        {
            if (studyId == null)
            {
                Console.WriteLine("ERROR: Study is null---------------------------------");
            }
            Console.WriteLine($"STUDYID: { studyId}----------------");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userStudyIds = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == userId)
                .Select(rs => rs.StudyId)
                .ToListAsync();

            // Fetch participants for the given study ID
            var participantIds = await _context.ParticipantStudies
                .Where(ps => ps.StudyId == studyId)
                .Select(ps => ps.ParticipantId)
                .Distinct()
                .ToListAsync();

            AvailableParticipants = await _context.Participants
                .Where(p => participantIds.Contains(p.ParticipantId))
                .OrderBy(p => p.ParticipantFirstName)
                .ThenBy(p => p.ParticipantLastName)
                .ToListAsync();

            AvailableSessions = await _context.Sessions
                .Where(s => userStudyIds.Contains(s.StudyId))
                .OrderBy(s => s.Date)
                .ToListAsync();


            if (startDate.HasValue && endDate.HasValue && participants.Any())
            {
                Console.WriteLine($"Filtering sessions from {startDate} to {endDate} for participants: {string.Join(", ", participants)}");

                var filteredSessions = AvailableSessions
                    .Where(s => s.Date >= DateOnly.FromDateTime(startDate.Value) && s.Date <= DateOnly.FromDateTime(endDate.Value))
                    .ToList();

                Console.WriteLine("Filtered Sessions:");
                foreach (var session in filteredSessions)
                {
                    Console.WriteLine($"- SessionId: {session.SessionId}, Date: {session.Date}");
                }

                var filteredAnswers = await _context.FormAnswers
                    .Where(a => filteredSessions.Select(s => s.SessionId).Contains(a.SessionId) &&
                                participants.Contains(a.ParticipantId))
                    .Include(a => a.FormQuestion)
                    .ToListAsync();

                Console.WriteLine($"FilteredAnswers Count: {filteredAnswers.Count}");

                // Load option values for answers with selected options
                var optionValues = await _context.FormQuestionOptions
                    .Where(o => filteredAnswers.Select(a => a.SelectedOption).Contains(o.OptionId))
                    .ToDictionaryAsync(o => o.OptionId, o => o.OptionValue);

                foreach (var answer in filteredAnswers)
                {
                    if (answer.SelectedOption != null && optionValues.TryGetValue(answer.SelectedOption, out var value))
                    {
                        Console.WriteLine($"Answer: ParticipantId={answer.ParticipantId}, SessionId={answer.SessionId}, Category={answer.FormQuestion.Category}, OptionValue={value}");
                    }
                }

                var groupedData = filteredAnswers
                    .Where(a => a.SelectedOption != null && optionValues.ContainsKey(a.SelectedOption))
                    .GroupBy(a => new { a.FormQuestion.Category, a.SessionId })
                    .Select(g => new
                    {
                        g.Key.Category,
                        g.Key.SessionId,
                        AverageScore = g.Sum(a => optionValues[a.SelectedOption]) / (double)participants.Count
                    })
                    .ToList();

                Console.WriteLine($"GroupedData Count: {groupedData.Count}");
                foreach (var group in groupedData)
                {
                    Console.WriteLine($"Category: {group.Category}, SessionId: {group.SessionId}, AverageScore: {group.AverageScore}");
                }

                var sessions = filteredSessions.OrderBy(s => s.Date).ToList();
                var sessionDates = sessions.Select(s => s.Date.ToString("yyyy-MM-dd")).ToList();
                var categories = groupedData.Select(g => g.Category).Distinct().ToList();

                var scoresByCategory = categories.Select(category =>
                    sessions.Select(session =>
                        groupedData
                            .Where(g => g.Category == category && g.SessionId == session.SessionId)
                            .Select(g => g.AverageScore)
                            .FirstOrDefault()
                    ).ToList()
                ).ToList();

                ChartDataJson = JsonConvert.SerializeObject(new
                {
                    labels = sessionDates,
                    categories,
                    scores = scoresByCategory,
                    colors = categories.Select(_ => GetRandomColor()).ToList()
                });

                Console.WriteLine($"ChartDataJson: {ChartDataJson}");
            }
            else
            {
                Console.WriteLine("Insufficient data to generate chart.");
            }
        }

        private string GetRandomColor()
        {
            var random = new Random();
            return $"rgba({random.Next(0, 255)}, {random.Next(0, 255)}, {random.Next(0, 255)}, 1)";
        }
    }
}
