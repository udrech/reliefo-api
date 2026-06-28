using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reliefo_api.Migrations
{
    /// <inheritdoc />
    public partial class MedicalHistoryMarkings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "markings",
                table: "medical_history_records",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "markings_image",
                table: "medical_history_records",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "markings",
                table: "medical_history_records");

            migrationBuilder.DropColumn(
                name: "markings_image",
                table: "medical_history_records");
        }
    }
}
