using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedWeekTypecolumninClassSchedulestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WeekType",
                table: "ClassSchedules",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekType",
                table: "ClassSchedules");
        }
    }
}
