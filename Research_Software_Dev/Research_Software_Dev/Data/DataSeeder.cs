using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Sessions;
using Research_Software_Dev.Models.Studies;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;

    public DataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedDataAsync()
    {
        Console.WriteLine("Starting data seeding process...");

        // Ensure the database exists and is up-to-date
        await _context.Database.MigrateAsync();
        Console.WriteLine("Database migration completed.");

        // Form ID to target
        var formId = "ea063fac-9d32-4d3b-9e6a-30d13db6b894";

        // Seed study
        var studyId = "d3d36e03-a11e-4ec4-9ee6-d0fce5a2d630";

        // Seed participants
        var participants = new List<Participant>();
        for (int i = 1; i <= 5; i++)
        {
            var participantId = Guid.NewGuid().ToString();
            participants.Add(new Participant
            {
                ParticipantId = participantId,
                ParticipantFirstName = $"Participant{i}",
                ParticipantLastName = $"LastName{i}"
            });

            if (!await _context.ParticipantStudies.AnyAsync(ps => ps.ParticipantId == participantId && ps.StudyId == studyId))
            {
                _context.ParticipantStudies.Add(new ParticipantStudy
                {
                    ParticipantId = participantId,
                    StudyId = studyId
                });
                Console.WriteLine($"Added ParticipantStudy for Participant {participantId}");
            }
        }

        foreach (var participant in participants)
        {
            if (!await _context.Participants.AnyAsync(p => p.ParticipantId == participant.ParticipantId))
            {
                _context.Participants.Add(participant);
                Console.WriteLine($"Added participant {participant.ParticipantId}");
            }
        }

        // Seed sessions
        var random = new Random();
        var startDate = new DateTime(2024, 10, 1);
        var endDate = new DateTime(2024, 11, 30);
        var sessions = new List<Session>();

        for (int i = 1; i <= 10; i++)
        {
            var sessionDate = startDate.AddDays(random.Next(0, (endDate - startDate).Days));
            var session = new Session
            {
                SessionId = Guid.NewGuid().ToString(),
                Date = DateOnly.FromDateTime(sessionDate),
                TimeStart = TimeOnly.FromDateTime(sessionDate.AddHours(9)),
                TimeEnd = TimeOnly.FromDateTime(sessionDate.AddHours(10)),
                StudyId = studyId
            };

            sessions.Add(session);
            if (!await _context.Sessions.AnyAsync(s => s.SessionId == session.SessionId))
            {
                _context.Sessions.Add(session);
                Console.WriteLine($"Added session {session.SessionId} on {sessionDate.ToShortDateString()}");
            }
        }

        await _context.SaveChangesAsync();

        // Link participants to sessions
        foreach (var session in sessions)
        {
            foreach (var participant in participants)
            {
                if (!await _context.ParticipantSessions.AnyAsync(ps => ps.ParticipantId == participant.ParticipantId && ps.SessionId == session.SessionId))
                {
                    _context.ParticipantSessions.Add(new ParticipantSession
                    {
                        ParticipantId = participant.ParticipantId,
                        SessionId = session.SessionId
                    });
                    Console.WriteLine($"Linked Participant {participant.ParticipantId} to Session {session.SessionId}");
                }
            }
        }

        // Link Amin to sessions
        foreach (var session in sessions)
        {
            _context.ResearcherSessions.Add(new ResearcherSession
            {
                ResearcherId = "c786e62d-d396-45e6-be89-207e528ab857",
                SessionId = session.SessionId
            });
        }

        await _context.SaveChangesAsync();

        // Get FormQuestions associated with the specified FormId
        var formQuestions = await _context.FormQuestions.Where(q => q.FormId == formId).ToListAsync();
        Console.WriteLine($"Found {formQuestions.Count} FormQuestions associated with FormId {formId}.");

        if (!formQuestions.Any())
        {
            Console.WriteLine("No FormQuestions found. Seeding aborted.");
            return;
        }

        // Seed answers
        var answers = new List<FormAnswer>();

        foreach (var session in sessions)
        {
            foreach (var participant in participants)
            {
                foreach (var question in formQuestions)
                {
                    var randomAnswer = random.Next(0, 4).ToString(); // Randomly pick from "0", "1", "2", "3"
                    answers.Add(new FormAnswer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        TextAnswer = randomAnswer,
                        TimeStamp = DateTime.Now,
                        ParticipantId = participant.ParticipantId,
                        SessionId = session.SessionId,
                        FormQuestionId = question.FormQuestionId
                    });
                    Console.WriteLine($"Prepared FormAnswer for Participant {participant.ParticipantId}, Session {session.SessionId}, Question {question.FormQuestionId} with Answer {randomAnswer}");
                }
            }
        }

        Console.WriteLine($"Prepared {answers.Count} FormAnswers for seeding.");

        if (answers.Any())
        {
            try
            {
                await _context.FormAnswers.AddRangeAsync(answers);
                await _context.SaveChangesAsync();
                Console.WriteLine("FormAnswers seeded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving FormAnswers: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("No FormAnswers to seed.");
        }

        Console.WriteLine("Data seeding process completed.");
    }
}
