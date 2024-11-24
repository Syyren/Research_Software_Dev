using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Roles;
using Research_Software_Dev.Models.Sessions;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

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
        public DbSet<ResearcherRole> ResearcherRoles { get; set; }
        public DbSet<ResearcherSession> ResearcherSessions { get; set; }
        public DbSet<ResearcherStudy> ResearcherStudies { get; set; }

        //Roles
        public DbSet<Role> Roles { get; set; }

        //Sessions
        public DbSet<Session> Sessions { get; set; }

        //Studies
        public DbSet<Study> Studies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //composite keys
                //Participants
            modelBuilder.Entity<ParticipantSession>()
                .HasKey(ps => new { ps.ParticipantId, ps.SessionId });
            modelBuilder.Entity<ParticipantStudy>()
                .HasKey(ps => new { ps.ParticipantId, ps.StudyId });
                //Researchers
            modelBuilder.Entity<ResearcherRole>()
                .HasKey(rr => new { rr.ResearcherId, rr.RoleId });
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

            //initial data for roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Audit" },
                new Role { RoleId = 3, RoleName = "Level 3" },
                new Role { RoleId = 4, RoleName = "Level 2" },
                new Role { RoleId = 5, RoleName = "Level 1" },
                new Role { RoleId = 6, RoleName = "Level 0" }
            );
        }
    }
}