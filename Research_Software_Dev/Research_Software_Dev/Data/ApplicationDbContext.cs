using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Models.Forms;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Research_Software_Dev.Models.Studies.Study> Study { get; set; } = default!;
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormQuestion> FormQuestions { get; set; }
        public DbSet<FormAnswer> FormAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FormQuestion>()
                .HasOne(fq => fq.Form)
                .WithMany(f => f.Questions)
                .HasForeignKey(fq => fq.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FormAnswer>()
                .HasOne(fa => fa.Question)
                .WithMany()
                .HasForeignKey(fa => fa.QuestionId);
        }
    }
}
