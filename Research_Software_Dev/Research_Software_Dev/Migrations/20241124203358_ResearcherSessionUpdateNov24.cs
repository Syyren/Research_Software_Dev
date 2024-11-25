using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    /// <inheritdoc />
    public partial class ResearcherSessionUpdateNov24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResearcherStudies_Researchers_Id",
                table: "ResearcherStudies");

            migrationBuilder.DropIndex(
                name: "IX_ResearcherStudies_Id",
                table: "ResearcherStudies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ResearcherStudies");

            migrationBuilder.DropColumn(
                name: "StudyName",
                table: "ResearcherStudies");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearcherStudies_Researchers_ResearcherId",
                table: "ResearcherStudies",
                column: "ResearcherId",
                principalTable: "Researchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "StudyName",
                table: "ResearcherStudies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ResearcherStudies_Id",
                table: "ResearcherStudies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResearcherStudies_Researchers_Id",
                table: "ResearcherStudies",
                column: "Id",
                principalTable: "Researchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
