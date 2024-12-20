﻿using Microsoft.AspNetCore.Identity;
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
        public DbSet<FormQuestionOption> FormQuestionOptions { get; set; }

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
            base.OnModelCreating(modelBuilder);

            // FormQuestion and FormQuestionOption Relationship
            modelBuilder.Entity<FormQuestion>()
                .HasMany(q => q.Options)
                .WithOne(o => o.FormQuestion)
                .HasForeignKey(o => o.FormQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // FormQuestion and Form Relationship
            modelBuilder.Entity<FormQuestion>()
                .HasOne(fq => fq.Form)
                .WithMany(f => f.Questions)
                .HasForeignKey(fq => fq.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // FormAnswer and FormQuestion Relationship
            modelBuilder.Entity<FormAnswer>()
                .HasOne(fa => fa.FormQuestion)
                .WithMany()
                .HasForeignKey(fa => fa.FormQuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // FormAnswer and ParticipantSession Relationship
            modelBuilder.Entity<FormAnswer>()
                .HasOne(fa => fa.ParticipantSession)
                .WithMany()
                .HasForeignKey(fa => new { fa.ParticipantId, fa.SessionId })
                .OnDelete(DeleteBehavior.Restrict);

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
