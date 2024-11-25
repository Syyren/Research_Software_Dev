using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Research_Software_Dev.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ResearcherStudies",
                keyColumns: new[] { "ResearcherId", "StudyId" },
                keyValues: new object[] { "Researcher1", "Study1" });

            migrationBuilder.InsertData(
                table: "ResearcherStudies",
                columns: new[] { "ResearcherId", "StudyId" },
                values: new object[] { "63c863ed-f363-46e7-9ef5-a2e4fd7c677d", "Study1" });

            migrationBuilder.UpdateData(
                table: "Researchers",
                keyColumn: "Id",
                keyValue: "Researcher1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2ca72ee5-32e9-44a5-9546-4d4007df0c28", "AQAAAAIAAYagAAAAEIMipTMk8YduFTiE58dnx04HHVKp/myV8KYMYdprqrUyoPeAr7MHOGMLJfUhdTcIFw==", "77370e8c-ae27-405c-b317-f746295946b5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ResearcherStudies",
                keyColumns: new[] { "ResearcherId", "StudyId" },
                keyValues: new object[] { "63c863ed-f363-46e7-9ef5-a2e4fd7c677d", "Study1" });

            migrationBuilder.InsertData(
                table: "ResearcherStudies",
                columns: new[] { "ResearcherId", "StudyId" },
                values: new object[] { "Researcher1", "Study1" });

            migrationBuilder.UpdateData(
                table: "Researchers",
                keyColumn: "Id",
                keyValue: "Researcher1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "113593d9-91a5-4a89-a2a1-cc579bfce9ee", "AQAAAAIAAYagAAAAEHk0i3Fnv5D+PGOtFyT4C9KntDmEl99NMz9kjhkqHwMZHa5tDxWZrFGCTtlsC1LMXA==", "0f6ae2c9-7dd9-4cbd-b31a-3300f6bb7056" });
        }
    }
}
