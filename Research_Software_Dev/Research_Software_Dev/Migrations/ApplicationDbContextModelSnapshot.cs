﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Research_Software_Dev.Data;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Forms.Form", b =>
                {
                    b.Property<string>("FormId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FormId");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Forms.FormAnswer", b =>
                {
                    b.Property<string>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FormQuestionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ParticipantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TextAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("AnswerId");

                    b.HasIndex("FormQuestionId");

                    b.HasIndex("ParticipantId", "SessionId");

                    b.ToTable("FormAnswers");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Forms.FormQuestion", b =>
                {
                    b.Property<string>("FormQuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FormId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OptionsJson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionNumber")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("FormQuestionId");

                    b.HasIndex("FormId");

                    b.ToTable("FormQuestions");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Participants.Participant", b =>
                {
                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ParticipantAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ParticipantEmail")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ParticipantFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ParticipantLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ParticipantPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParticipantId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Participants.ParticipantSession", b =>
                {
                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SessionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ParticipantId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("ParticipantSessions");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Participants.ParticipantStudy", b =>
                {
                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StudyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ParticipantId", "StudyId");

                    b.HasIndex("StudyId");

                    b.ToTable("ParticipantStudies");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Researchers.Researcher", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ResearcherAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ResearcherFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ResearcherLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Researchers", (string)null);
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Researchers.ResearcherSession", b =>
                {
                    b.Property<string>("ResearcherId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SessionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ResearcherId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("ResearcherSessions");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Researchers.ResearcherStudy", b =>
                {
                    b.Property<string>("ResearcherId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StudyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ResearcherId", "StudyId");

                    b.HasIndex("StudyId");

                    b.ToTable("ResearcherStudies");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Sessions.Session", b =>
                {
                    b.Property<string>("SessionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("StudyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeOnly>("TimeEnd")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("TimeStart")
                        .HasColumnType("time");

                    b.HasKey("SessionId");

                    b.HasIndex("StudyId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Studies.Study", b =>
                {
                    b.Property<string>("StudyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StudyDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudyId");

                    b.ToTable("Studies");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Researchers.Researcher", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Researchers.Researcher", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Research_Software_Dev.Models.Researchers.Researcher", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Researchers.Researcher", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Forms.FormAnswer", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Forms.FormQuestion", "FormQuestion")
                        .WithMany()
                        .HasForeignKey("FormQuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Research_Software_Dev.Models.Participants.ParticipantSession", "ParticipantSession")
                        .WithMany()
                        .HasForeignKey("ParticipantId", "SessionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FormQuestion");

                    b.Navigation("ParticipantSession");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Forms.FormQuestion", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Forms.Form", "Form")
                        .WithMany("Questions")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Participants.ParticipantSession", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Participants.Participant", "Participant")
                        .WithMany()
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Research_Software_Dev.Models.Sessions.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Participants.ParticipantStudy", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Participants.Participant", "Participant")
                        .WithMany()
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Research_Software_Dev.Models.Studies.Study", "Study")
                        .WithMany()
                        .HasForeignKey("StudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");

                    b.Navigation("Study");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Researchers.ResearcherSession", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Researchers.Researcher", "Researcher")
                        .WithMany()
                        .HasForeignKey("ResearcherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Research_Software_Dev.Models.Sessions.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Researcher");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Researchers.ResearcherStudy", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Researchers.Researcher", "Researcher")
                        .WithMany()
                        .HasForeignKey("ResearcherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Research_Software_Dev.Models.Studies.Study", "Study")
                        .WithMany()
                        .HasForeignKey("StudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Researcher");

                    b.Navigation("Study");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Sessions.Session", b =>
                {
                    b.HasOne("Research_Software_Dev.Models.Studies.Study", "Study")
                        .WithMany()
                        .HasForeignKey("StudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Study");
                });

            modelBuilder.Entity("Research_Software_Dev.Models.Forms.Form", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
