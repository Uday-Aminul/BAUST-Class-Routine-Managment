using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedtheseededdataforClassScheduletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClassSchedules",
                columns: new[] { "Id", "ClassroomId", "CourseId", "Day", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[] { 16, null, null, 0, new TimeOnly(15, 20, 0), 411, 30, new TimeOnly(14, 30, 0), 33 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
