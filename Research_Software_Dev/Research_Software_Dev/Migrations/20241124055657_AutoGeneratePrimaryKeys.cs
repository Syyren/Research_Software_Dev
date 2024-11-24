using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    /// <inheritdoc />
    public partial class AutoGeneratePrimaryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormAnswers_FormQuestions_QuestionId",
                table: "FormAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_FormAnswers_FormQuestions_QuestionId",
                table: "FormAnswers",
                column: "QuestionId",
                principalTable: "FormQuestions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormAnswers_FormQuestions_QuestionId",
                table: "FormAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_FormAnswers_FormQuestions_QuestionId",
                table: "FormAnswers",
                column: "QuestionId",
                principalTable: "FormQuestions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
