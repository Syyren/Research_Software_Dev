using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Research_Software_Dev.Pages.Data
{
    public class BubbleChartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BubbleChartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string StudyId { get; set; }

        public List<Session> AvailableSessions { get; set; } = new List<Session>();
        public string ChartDataJson { get; private set; }

        public async Task OnGetAsync()
        {
            Console.WriteLine("Fetching data for Bubble Chart...");

            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("ERROR: User ID is null.");
                return;
            }

            // Fetch studies linked to the current user
            var userStudyIds = await _context.ResearcherStudies
                .Where(rs => rs.ResearcherId == userId)
                .Select(rs => rs.StudyId)
                .ToListAsync();

            Console.WriteLine($"User Study IDs: {string.Join(", ", userStudyIds)}");

            // Fetch participants linked to the user's studies
            var userParticipantIds = await _context.ParticipantStudies
                .Where(ps => userStudyIds.Contains(ps.StudyId))
                .Select(ps => ps.ParticipantId)
                .Distinct()
                .ToListAsync();

            Console.WriteLine($"User Participant IDs: {string.Join(", ", userParticipantIds)}");

            // Fetch sessions linked to the participants through a linking table
            var userSessionIds = await _context.ParticipantSessions
                .Where(ps => userParticipantIds.Contains(ps.ParticipantId))
                .Select(ps => ps.SessionId)
                .Distinct()
                .ToListAsync();

            Console.WriteLine($"User Session IDs: {string.Join(", ", userSessionIds)}");

            // Fetch sessions based on the session IDs
            AvailableSessions = await _context.Sessions
                .Where(s => userSessionIds.Contains(s.SessionId))
                .OrderBy(s => s.Date)
                .ToListAsync();

            Console.WriteLine("Filtered Available Sessions:");
            foreach (var session in AvailableSessions)
            {
                Console.WriteLine($"- SessionId: {session.SessionId}, Date: {session.Date}");
            }

            // Fetch attendance and FormAnswer data for filtered sessions
            var answers = await _context.FormAnswers
                .Where(a => userSessionIds.Contains(a.SessionId))
                .Include(a => a.FormQuestion)
                .ToListAsync();

            var participants = await _context.Participants
                .Where(p => userParticipantIds.Contains(p.ParticipantId))
                .ToListAsync();

            // Calculate average FormAnswers per participant per session
            var data = answers
                .GroupBy(a => new { a.ParticipantId, a.SessionId })
                .Select(g => new
                {
                    ParticipantId = g.Key.ParticipantId,
                    SessionId = g.Key.SessionId,
                    AverageAnswers = g.Count() // Average answers
                })
                .ToList();

            // Match participant names and session dates
            var participantNames = participants.ToDictionary(p => p.ParticipantId, p => p.ParticipantFirstName + " " + p.ParticipantLastName);
            var sessionDates = AvailableSessions.ToDictionary(s => s.SessionId, s => s.Date.ToDateTime(TimeOnly.MinValue).ToOADate()); // Convert to numeric values for x-axis

            // Prepare Chart.js data
            var bubbles = data.Select(d => new
            {
                x = sessionDates[d.SessionId], // Session date as numeric value
                y = participantNames[d.ParticipantId], // Participant name
                r = d.AverageAnswers // Bubble size
            });

            ChartDataJson = JsonConvert.SerializeObject(bubbles);

            Console.WriteLine($"ChartDataJson: {ChartDataJson}");
        }

    }
}
