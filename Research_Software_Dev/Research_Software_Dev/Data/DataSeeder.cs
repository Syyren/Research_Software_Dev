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

        // DASS21 Form
        var formId = "dass21-0001";
        var formName = "DASS21";

        if (!await _context.Forms.AnyAsync(f => f.FormId == formId))
        {
            var dassQuestions = new List<FormQuestion>
            {
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 1, QuestionDescription = "I found it hard to wind down", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 2, QuestionDescription = "I was aware of dryness of my mouth", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 3, QuestionDescription = "I couldn’t seem to experience any positive feeling at all", Type = QuestionType.LikertScale, Category = "Depression" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 4, QuestionDescription = "I experienced breathing difficulty (e.g., excessively rapid breathing, breathlessness in the absence of physical exertion)", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 5, QuestionDescription = "I found it difficult to work up the initiative to do things", Type = QuestionType.LikertScale, Category = "Depression" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 6, QuestionDescription = "I tended to over-react to situations", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 7, QuestionDescription = "I experienced trembling (e.g., in the hands)", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 8, QuestionDescription = "I felt that I was using a lot of nervous energy", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 9, QuestionDescription = "I was worried about situations in which I might panic and make a fool of myself", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 10, QuestionDescription = "I felt that I had nothing to look forward to", Type = QuestionType.LikertScale, Category = "Depression" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 11, QuestionDescription = "I found myself getting agitated", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 12, QuestionDescription = "I found it difficult to relax", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 13, QuestionDescription = "I felt down-hearted and blue", Type = QuestionType.LikertScale, Category = "Depression" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 14, QuestionDescription = "I was intolerant of anything that kept me from getting on with what I was doing", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 15, QuestionDescription = "I felt I was close to panic", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 16, QuestionDescription = "I was unable to become enthusiastic about anything", Type = QuestionType.LikertScale, Category = "Depression" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 17, QuestionDescription = "I felt I wasn’t worth much as a person", Type = QuestionType.LikertScale, Category = "Depression" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 18, QuestionDescription = "I felt that I was rather touchy", Type = QuestionType.LikertScale, Category = "Stress" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 19, QuestionDescription = "I was aware of the action of my heart in the absence of physical exertion (e.g., sense of heart rate increase, heart missing a beat)", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 20, QuestionDescription = "I felt scared without any good reason", Type = QuestionType.LikertScale, Category = "Anxiety" },
                new FormQuestion { FormQuestionId = Guid.NewGuid().ToString(), FormId = formId, QuestionNumber = 21, QuestionDescription = "I felt that life was meaningless", Type = QuestionType.LikertScale, Category = "Depression" }
            };

            foreach (var question in dassQuestions)
            {
                question.Options = new List<FormQuestionOption>
                {
                    new FormQuestionOption { OptionId = Guid.NewGuid().ToString(), FormQuestionId = question.FormQuestionId, OptionText = "Did not apply to me at all", OptionValue = 0 },
                    new FormQuestionOption { OptionId = Guid.NewGuid().ToString(), FormQuestionId = question.FormQuestionId, OptionText = "Applied to me to some degree", OptionValue = 1 },
                    new FormQuestionOption { OptionId = Guid.NewGuid().ToString(), FormQuestionId = question.FormQuestionId, OptionText = "Applied to me to a considerable degree", OptionValue = 2 },
                    new FormQuestionOption { OptionId = Guid.NewGuid().ToString(), FormQuestionId = question.FormQuestionId, OptionText = "Applied to me very much", OptionValue = 3 }
                };
            }

            var dass21Form = new Form
            {
                FormId = formId,
                FormName = formName,
                Questions = dassQuestions
            };

            _context.Forms.Add(dass21Form);
            Console.WriteLine($"DASS21 form with {dassQuestions.Count} questions seeded.");
        }

        // Seed study
        var studyId = "1b718117-50fe-4216-bd18-29971220fee5";

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
                ResearcherId = "1aa84535-0570-4504-b39f-99b1ecea1d2c",
                SessionId = session.SessionId
            });
        }

        await _context.SaveChangesAsync();

        // Seed answers for DASS21
        var dass21Questions = await _context.FormQuestions.Where(q => q.FormId == formId).ToListAsync();

        if (!dass21Questions.Any())
        {
            Console.WriteLine("No questions found for DASS21 form. Seeding aborted.");
            return;
        }

        // Seed answers
        // Dynamically fetch FormQuestions from the database for the specific form
        var formQuestions = await _context.FormQuestions
            .Where(q => q.FormId == "dass21-0001") // Replace with the actual Form ID for DASS21
            .Include(q => q.Options) // Include related options
            .ToListAsync();

        if (!formQuestions.Any())
        {
            Console.WriteLine($"No FormQuestions found for Form ID {"dass21-0001"}. Seeding aborted.");
            return;
        }

        Console.WriteLine($"Found {formQuestions.Count} FormQuestions for Form ID {"dass21-0001"}.");

        // Seed answers
        var answers = new List<FormAnswer>();

        foreach (var session in sessions)
        {
            foreach (var participant in participants)
            {
                foreach (var question in formQuestions)
                {
                    // Query all options associated with the question
                    var options = question.Options.ToList(); // Use preloaded options to minimize database calls

                    if (options.Any())
                    {
                        // Pick a random option from the available options
                        var randomOption = options[random.Next(options.Count)];

                        answers.Add(new FormAnswer
                        {
                            AnswerId = Guid.NewGuid().ToString(),
                            TextAnswer = randomOption.OptionText, // Set the answer text to the option's text
                            SelectedOption = randomOption.OptionId, // Store the selected option ID
                            TimeStamp = DateTime.UtcNow,
                            ParticipantId = participant.ParticipantId,
                            SessionId = session.SessionId,
                            FormQuestionId = question.FormQuestionId
                        });

                        Console.WriteLine($"Prepared FormAnswer for Participant {participant.ParticipantId}, Session {session.SessionId}, Question {question.FormQuestionId} with Option {randomOption.OptionId}");
                    }
                    else
                    {
                        Console.WriteLine($"No options available for Question {question.FormQuestionId}. Skipping.");
                    }
                }
            }
        }



        if (answers.Any())
        {
            await _context.FormAnswers.AddRangeAsync(answers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Seeded {answers.Count} DASS21 answers successfully.");
        }

        Console.WriteLine("Data seeding process completed.");
    }
}
