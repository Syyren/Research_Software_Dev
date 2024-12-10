﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToFormQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "FormQuestions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FormQuestions");
        }
    }
}
