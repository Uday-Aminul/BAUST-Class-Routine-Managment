using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class ClassSchedulesseeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClassSchedules",
                columns: new[] { "Id", "ClassroomId", "CourseId", "Day", "EndTime", "LabroomId", "SessionalStatus", "StartTime", "TeacherId" },
                values: new object[,]
                {
                    { 4001, 304, 501, 1, new TimeOnly(10, 30, 0), null, null, new TimeOnly(9, 0, 0), 101 },
                    { 4002, 304, 502, 3, new TimeOnly(12, 30, 0), null, null, new TimeOnly(11, 0, 0), 101 },
                    { 4003, 304, 503, 2, new TimeOnly(11, 30, 0), null, null, new TimeOnly(10, 0, 0), 102 },
                    { 4004, 305, 504, 4, new TimeOnly(14, 30, 0), null, null, new TimeOnly(13, 0, 0), 102 },
                    { 4005, 305, 501, 0, new TimeOnly(10, 30, 0), null, null, new TimeOnly(9, 0, 0), 102 },
                    { 4006, 306, 502, 2, new TimeOnly(15, 30, 0), null, null, new TimeOnly(14, 0, 0), 104 },
                    { 4007, 306, 503, 1, new TimeOnly(10, 0, 0), null, null, new TimeOnly(8, 30, 0), 105 },
                    { 4008, 308, 504, 3, new TimeOnly(13, 30, 0), null, null, new TimeOnly(12, 0, 0), 103 },
                    { 4009, 308, 501, 4, new TimeOnly(16, 30, 0), null, null, new TimeOnly(15, 0, 0), 105 },
                    { 4010, 308, 502, 0, new TimeOnly(11, 30, 0), null, null, new TimeOnly(10, 0, 0), 104 },
                    { 4011, 308, 503, 2, new TimeOnly(12, 30, 0), null, null, new TimeOnly(11, 0, 0), 105 },
                    { 4012, 309, 504, 1, new TimeOnly(14, 30, 0), null, null, new TimeOnly(13, 0, 0), 101 },
                    { 4013, 309, 501, 3, new TimeOnly(11, 0, 0), null, null, new TimeOnly(9, 30, 0), 102 },
                    { 4014, 310, 502, 4, new TimeOnly(15, 30, 0), null, null, new TimeOnly(14, 0, 0), 103 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4006);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4007);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4008);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4009);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4010);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4011);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4012);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4013);

            migrationBuilder.DeleteData(
                table: "ClassSchedules",
                keyColumn: "Id",
                keyValue: 4014);
        }
    }
}
