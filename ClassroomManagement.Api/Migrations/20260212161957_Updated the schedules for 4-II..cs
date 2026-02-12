using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Updatedtheschedulesfor4II : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClassroomId", "CourseId", "Day", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[] { null, null, 0, new TimeOnly(8, 50, 0), 307, 31, new TimeOnly(8, 0, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 33, new TimeOnly(10, 50, 0), new TimeOnly(10, 0, 0), 39 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 39, new TimeOnly(12, 20, 0), new TimeOnly(11, 30, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ClassroomId", "Day", "EndTime", "StartTime" },
                values: new object[] { 310, 1, new TimeOnly(13, 20, 0), new TimeOnly(12, 30, 0) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 41, new TimeOnly(8, 50, 0), new TimeOnly(8, 0, 0), 38 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 39, new TimeOnly(9, 50, 0), new TimeOnly(9, 0, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 40, new TimeOnly(10, 50, 0), new TimeOnly(10, 0, 0), 33 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CourseId", "Day", "TeacherId" },
                values: new object[] { 33, 2, 39 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 42, new TimeOnly(12, 20, 0), new TimeOnly(11, 30, 0), 40 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 40, new TimeOnly(13, 20, 0), new TimeOnly(12, 30, 0), 33 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CourseId", "Day", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 33, 3, new TimeOnly(14, 20, 0), new TimeOnly(13, 30, 0), 39 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 40, new TimeOnly(8, 50, 0), new TimeOnly(8, 0, 0), 33 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 39, new TimeOnly(9, 50, 0), new TimeOnly(9, 0, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 42, new TimeOnly(10, 50, 0), new TimeOnly(10, 0, 0), 40 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ClassroomId", "CourseId", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[] { 305, 43, new TimeOnly(12, 20, 0), null, null, new TimeOnly(11, 30, 0), 41 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClassroomId", "CourseId", "Day", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[] { 407, 33, 1, new TimeOnly(10, 50, 0), null, null, new TimeOnly(10, 0, 0), 39 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 39, new TimeOnly(12, 20, 0), new TimeOnly(11, 30, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 41, new TimeOnly(13, 20, 0), new TimeOnly(12, 30, 0), 38 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ClassroomId", "Day", "EndTime", "StartTime" },
                values: new object[] { 407, 2, new TimeOnly(8, 50, 0), new TimeOnly(8, 0, 0) });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 39, new TimeOnly(9, 50, 0), new TimeOnly(9, 0, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 40, new TimeOnly(10, 50, 0), new TimeOnly(10, 0, 0), 33 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 33, new TimeOnly(12, 20, 0), new TimeOnly(11, 30, 0), 39 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CourseId", "Day", "TeacherId" },
                values: new object[] { 42, 3, 40 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 40, new TimeOnly(13, 20, 0), new TimeOnly(12, 30, 0), 33 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 33, new TimeOnly(14, 20, 0), new TimeOnly(13, 30, 0), 39 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CourseId", "Day", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 40, 4, new TimeOnly(8, 50, 0), new TimeOnly(8, 0, 0), 33 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 39, new TimeOnly(9, 50, 0), new TimeOnly(9, 0, 0), 31 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 42, new TimeOnly(10, 50, 0), new TimeOnly(10, 0, 0), 40 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CourseId", "EndTime", "StartTime", "TeacherId" },
                values: new object[] { 43, new TimeOnly(12, 20, 0), new TimeOnly(11, 30, 0), 41 });

            migrationBuilder.UpdateData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ClassroomId", "CourseId", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[] { null, null, new TimeOnly(3, 20, 0), 411, 30, new TimeOnly(2, 30, 0), 33 });

            migrationBuilder.InsertData(
                table: "ClassSchedules",
                columns: new[] { "Id", "ClassroomId", "CourseId", "Day", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[] { 16, null, null, 0, new TimeOnly(8, 50, 0), 307, 31, new TimeOnly(8, 0, 0), 31 });
        }
    }
}
