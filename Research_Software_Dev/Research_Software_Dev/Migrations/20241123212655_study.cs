using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    /// <inheritdoc />
    public partial class study : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Study",
                columns: table => new
                {
                    StudyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Study", x => x.StudyId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Study");
        }
    }
}
