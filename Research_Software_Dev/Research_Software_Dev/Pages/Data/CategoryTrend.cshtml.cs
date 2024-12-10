using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Research_Software_Dev.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;

namespace Research_Software_Dev.Pages.Data
{
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
        public string StartSession { get; set; }

        [BindProperty]
        public string EndSession { get; set; }

        public SelectList Studies { get; set; } = new SelectList(new List<SelectListItem>());
        public List<Participant> AvailableParticipants { get; set; } = new();
        public List<Session> AvailableSessions { get; set; } = new();
        public string ChartDataJson { get; private set; }

        public async Task OnGetAsync(string studyId, string startSession, string endSession, List<string> participants)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userStudyIds = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == userId)
                .Select(rs => rs.StudyId)
                .ToListAsync();

            var studies = await _context.Studies
                .Where(s => userStudyIds.Contains(s.StudyId))
                .ToListAsync();
            Studies = new SelectList(studies, "StudyId", "StudyName");

            var participantIds = await _context.ParticipantStudies
                .Where(ps => userStudyIds.Contains(ps.StudyId))
                .Select(ps => ps.ParticipantId)
                .Distinct()
                .ToListAsync();

            AvailableParticipants = await _context.Participants
                .Where(p => participantIds.Contains(p.ParticipantId))
                .ToListAsync();

            AvailableSessions = await _context.Sessions
                .Where(s => userStudyIds.Contains(s.StudyId))
                .OrderBy(s => s.Date)
                .ToListAsync();

            if (!string.IsNullOrEmpty(startSession) && !string.IsNullOrEmpty(endSession) && participants.Any())
            {
                var sessionDatesInRange = AvailableSessions
                    .Where(s => s.Date >= AvailableSessions.First(a => a.SessionId == startSession).Date &&
                                s.Date <= AvailableSessions.First(a => a.SessionId == endSession).Date)
                    .ToList();

                var sessionDatesMap = sessionDatesInRange
                    .ToDictionary(s => s.SessionId, s => s.Date);

                var orderedSessions = sessionDatesInRange.OrderBy(s => s.Date).ToList();
                var sessionIdsInRangeOrdered = orderedSessions.Select(s => s.SessionId).ToList();
                var sessionDates = orderedSessions.Select(s => s.Date.ToString("yyyy-MM-dd")).ToList();

                var filteredAnswers = (await _context.FormAnswers
                    .Where(a => sessionIdsInRangeOrdered.Contains(a.SessionId) &&
                                participants.Contains(a.ParticipantId))
                    .Include(a => a.FormQuestion)
                    .ToListAsync())
                    .Where(a => int.TryParse(a.TextAnswer, out _))
                    .ToList();

                var groupedData = filteredAnswers
                    .GroupBy(a => new { a.FormQuestion.Category, a.SessionId })
                    .Select(g => new
                    {
                        g.Key.Category,
                        g.Key.SessionId,
                        TotalScore = g.Sum(a => int.Parse(a.TextAnswer))
                    })
                    .ToList();

                var categories = groupedData.Select(g => g.Category).Distinct().ToList();

                var scoresByCategory = categories.Select(category =>
                    sessionIdsInRangeOrdered.Select(sessionId =>
                    {
                        var matchingGroups = groupedData
                            .Where(g => g.Category == category && g.SessionId == sessionId)
                            .ToList();

                        if (matchingGroups.Any())
                        {
                            var totalScore = matchingGroups.Sum(g => g.TotalScore);
                            var average = (double)totalScore / participants.Count;
                            return average;
                        }

                        return 0.0;
                    }).ToList()
                ).ToList();

                ChartDataJson = JsonConvert.SerializeObject(new
                {
                    labels = sessionDates, // Use ordered session dates
                    categories,
                    scores = scoresByCategory,
                    colors = categories.Select(_ => GetRandomColor()).ToList()
                });
            }
        }

        private string GetRandomColor()
        {
            var random = new Random();
            return $"rgba({random.Next(0, 255)}, {random.Next(0, 255)}, {random.Next(0, 255)}, 1)";
        }
    }
}
