using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNov24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearcherSessions_Researchers_ResearcherId",
                table: "ResearcherSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearcherStudies_Researchers_ResearcherId",
                table: "ResearcherStudies");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ResearcherStudies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ResearcherSessions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ResearcherStudies_Id",
                table: "ResearcherStudies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ResearcherSessions_Id",
                table: "ResearcherSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearcherSessions_Researchers_Id",
                table: "ResearcherSessions",
                column: "Id",
                principalTable: "Researchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearcherStudies_Researchers_Id",
                table: "ResearcherStudies",
                column: "Id",
                principalTable: "Researchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearcherSessions_Researchers_Id",
                table: "ResearcherSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_ResearcherStudies_Researchers_Id",
                table: "ResearcherStudies");

            migrationBuilder.DropIndex(
                name: "IX_ResearcherStudies_Id",
                table: "ResearcherStudies");

            migrationBuilder.DropIndex(
                name: "IX_ResearcherSessions_Id",
                table: "ResearcherSessions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ResearcherStudies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ResearcherSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearcherSessions_Researchers_ResearcherId",
                table: "ResearcherSessions",
                column: "ResearcherId",
                principalTable: "Researchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResearcherStudies_Researchers_ResearcherId",
                table: "ResearcherStudies",
                column: "ResearcherId",
                principalTable: "Researchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
