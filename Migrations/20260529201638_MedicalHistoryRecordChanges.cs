using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reliefo_api.Migrations
{
    /// <inheritdoc />
    public partial class MedicalHistoryRecordChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "medical_history_records",
                newName: "history_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "history_type",
                table: "medical_history_records",
                newName: "type");
        }
    }
}
