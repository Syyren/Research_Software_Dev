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

        //Forms
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormAnswer> FormAnswers { get; set; }
        public DbSet<FormQuestion> FormQuestions { get; set; }

        //Participants
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantSession> ParticipantSessions { get; set; }
        public DbSet<ParticipantStudy> ParticipantStudies { get; set; }

        //Researchers
        public DbSet<Researcher> Researchers { get; set; }
        public DbSet<ResearcherSession> ResearcherSessions { get; set; }
        public DbSet<ResearcherStudy> ResearcherStudies { get; set; }

        //Sessions
        public DbSet<Session> Sessions { get; set; }

        //Studies
        public DbSet<Study> Studies { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<Researcher, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Rename AspNetUsers Table to Researchers
            modelBuilder.Entity<Researcher>().ToTable("Researchers");

            //composite keys
                //Participants
            modelBuilder.Entity<ParticipantSession>()
                .HasKey(ps => new { ps.ParticipantId, ps.SessionId });
            modelBuilder.Entity<ParticipantStudy>()
                .HasKey(ps => new { ps.ParticipantId, ps.StudyId });
                
            //Researchers
            modelBuilder.Entity<ResearcherSession>()
                .HasKey(rr => new { rr.ResearcherId, rr.SessionId });
            modelBuilder.Entity<ResearcherStudy>()
                .HasKey(rr => new { rr.ResearcherId, rr.StudyId });

            // configure cascade delete for participant drop-out
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

            modelBuilder.Entity<FormAnswer>()
                .HasOne(fa => fa.ParticipantSession)
                .WithMany()
                .HasForeignKey(fa => new { fa.ParticipantId, fa.SessionId })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}