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

        public List<Session> AvailableSessions { get; set; } = new List<Session>();
        public string ChartDataJson { get; private set; }

        public async Task OnGetAsync()
        {
            Console.WriteLine("Fetching data for Bubble Chart...");

            // Load all available sessions
            AvailableSessions = await _context.Sessions
                .OrderBy(s => s.Date)
                .ToListAsync();

            Console.WriteLine("Available Sessions:");
            foreach (var session in AvailableSessions)
            {
                Console.WriteLine($"- SessionId: {session.SessionId}, Date: {session.Date}");
            }

            // Fetch attendance and FormAnswer data
            var answers = await _context.FormAnswers
                .Include(a => a.FormQuestion)
                .ToListAsync();

            var participants = await _context.Participants.ToListAsync();

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
