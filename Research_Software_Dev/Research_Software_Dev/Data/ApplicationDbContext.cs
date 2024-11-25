using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Sessions;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Data
{
    public class ApplicationDbContext : IdentityDbContext<Researcher>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Forms
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormAnswer> FormAnswers { get; set; }
        public DbSet<FormQuestion> FormQuestions { get; set; }

        // Participants
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantSession> ParticipantSessions { get; set; }
        public DbSet<ParticipantStudy> ParticipantStudies { get; set; }

        // Researchers
        public DbSet<Researcher> Researchers { get; set; }
        public DbSet<ResearcherSession> ResearcherSessions { get; set; }
        public DbSet<ResearcherStudy> ResearcherStudies { get; set; }

        // Sessions
        public DbSet<Session> Sessions { get; set; }

        // Studies
        public DbSet<Study> Studies { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<Researcher, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Researcher
            modelBuilder.Entity<Researcher>().HasData(
                new Researcher
                {
                    Id = "Researcher1",
                    ResearcherFirstName = "Alice",
                    ResearcherLastName = "Smith",
                    ResearcherAddress = "456 Science Road",
                    Email = "alice.smith@example.com",
                    PhoneNumber = "555-5678",
                    UserName = "alice.smith@example.com",
                    NormalizedUserName = "ALICE.SMITH@EXAMPLE.COM",
                    NormalizedEmail = "ALICE.SMITH@EXAMPLE.COM",
                    PasswordHash = new PasswordHasher<Researcher>().HashPassword(null, "Password123!")
                }
            );

            modelBuilder.Entity<Study>().HasData(
                new Study
                {
                    StudyId = "Study1",
                    StudyName = "Health Study",
                    StudyDescription = "A study focused on health and wellness."
                },
                new Study
                {
                    StudyId = "Study2",
                    StudyName = "Cognitive Study",
                    StudyDescription = "A study focused on cognitive development."
                }
            );

            // Seed Participant
            modelBuilder.Entity<Participant>().HasData(
                new Participant
                {
                    ParticipantId = "P1",
                    ParticipantFirstName = "John",
                    ParticipantLastName = "Doe",
                    ParticipantAddress = "123 Main Street",
                    ParticipantEmail = "johndoe@example.com",
                    ParticipantPhoneNumber = "555-1234"
                }
            );

            // Seed Session
            modelBuilder.Entity<Session>().HasData(
                new Session
                {
                    SessionId = "Session1",
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    TimeStart = new TimeOnly(9, 0),
                    TimeEnd = new TimeOnly(10, 0),
                    StudyId = "Study1" // Ensure this StudyId exists in your Study table
                }
            );

            // Seed ResearcherStudy
            modelBuilder.Entity<ResearcherStudy>().HasData(
                new ResearcherStudy
                {
                    ResearcherId = "63c863ed-f363-46e7-9ef5-a2e4fd7c677d",
                    StudyId = "Study1"
                }
            );

            base.OnModelCreating(modelBuilder);

            // Forms and Questions Relationship
            modelBuilder.Entity<FormQuestion>()
                .HasOne(fq => fq.Form)
                .WithMany(f => f.Questions)
                .HasForeignKey(fq => fq.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // Answers and Questions Relationship
            modelBuilder.Entity<FormAnswer>()
                .HasOne(fa => fa.FormQuestion)
                .WithMany()
                .HasForeignKey(fa => fa.FormQuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents multiple cascade paths from FormQuestion.

            // Answers and ParticipantSessions Relationship
            modelBuilder.Entity<FormAnswer>()
                .HasOne(fa => fa.ParticipantSession)
                .WithMany()
                .HasForeignKey(fa => new { fa.ParticipantId, fa.SessionId })
                .OnDelete(DeleteBehavior.Restrict); // Restricted to avoid cascade path conflicts.

            // Composite Keys
            modelBuilder.Entity<ParticipantSession>()
                .HasKey(ps => new { ps.ParticipantId, ps.SessionId });

            modelBuilder.Entity<ParticipantStudy>()
                .HasKey(ps => new { ps.ParticipantId, ps.StudyId });

            modelBuilder.Entity<ResearcherSession>()
                .HasKey(rr => new { rr.ResearcherId, rr.SessionId });

            modelBuilder.Entity<ResearcherStudy>()
                .HasKey(rr => new { rr.ResearcherId, rr.StudyId });

            // Cascade Deletes for Participant Drop-Out
            modelBuilder.Entity<ParticipantSession>()
                .HasOne(ps => ps.Participant)
                .WithMany()
                .HasForeignKey(ps => ps.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParticipantStudy>()
                .HasOne(ps => ps.Participant)
                .WithMany()
                .HasForeignKey(ps => ps.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rename AspNetUsers Table to Researchers
            modelBuilder.Entity<Researcher>().ToTable("Researchers");
        }
    }
}