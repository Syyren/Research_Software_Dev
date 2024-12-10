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

        public async Task OnGetAsync(string participantId, string sessionId)
        {
            Console.WriteLine("Fetching data for Session Category Chart...");

            // Populate participants
            AvailableParticipants = await _context.Participants.ToListAsync();
            Console.WriteLine($"Available Participants: {AvailableParticipants.Count}");

            // Populate sessions
            AvailableSessions = await _context.Sessions.OrderBy(s => s.Date).ToListAsync();
            Console.WriteLine($"Available Sessions: {AvailableSessions.Count}");

            // Check if participant and session are selected
            if (!string.IsNullOrEmpty(participantId) && !string.IsNullOrEmpty(sessionId))
            {
                Console.WriteLine($"Generating chart for Participant: {participantId}, Session: {sessionId}");

                var answers = await _context.FormAnswers
                    .Where(a => a.ParticipantId == participantId && a.SessionId == sessionId)
                    .Include(a => a.FormQuestion)
                    .ToListAsync();

                Console.WriteLine($"Fetched {answers.Count} answers");

                var groupedData = answers
                    .GroupBy(a => a.FormQuestion.Category)
                    .Select(g => new
                    {
                        Category = g.Key,
                        Score = g.Sum(a => int.Parse(a.TextAnswer))
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
