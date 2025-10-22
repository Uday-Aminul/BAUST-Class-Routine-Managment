using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLab = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    Credit = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "IsLab" },
                values: new object[,]
                {
                    { 301, true },
                    { 302, false },
                    { 303, false },
                    { 304, false },
                    { 305, false },
                    { 306, false },
                    { 307, false },
                    { 308, false },
                    { 309, false },
                    { 310, false },
                    { 311, true }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Designation", "Name" },
                values: new object[,]
                {
                    { 2001, "Assistant Professor", "Alice Smith" },
                    { 2002, "Associate Professor", "Bob Johnson" },
                    { 2003, "Professor", "Carol Williams" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Credit", "Level", "Name", "TeacherId", "Term" },
                values: new object[,]
                {
                    { 501, 3, 3, "Data Structures", 2001, 1 },
                    { 502, 3, 3, "Algorithms", 2001, 2 },
                    { 503, 3, 3, "Circuit Analysis", 2002, 1 },
                    { 504, 3, 3, "Electromagnetics", 2003, 2 }
                });

            migrationBuilder.InsertData(
                table: "ClassSchedules",
                columns: new[] { "Id", "ClassroomId", "CourseId", "Day", "EndTime", "Section", "StartTime", "TeacherId" },
                values: new object[,]
                {
                    { 4001, 302, 501, 1, new TimeOnly(10, 30, 0), "A", new TimeOnly(9, 0, 0), 2001 },
                    { 4002, 303, 502, 3, new TimeOnly(12, 30, 0), "A", new TimeOnly(11, 0, 0), 2001 },
                    { 4003, 304, 503, 2, new TimeOnly(11, 30, 0), "B", new TimeOnly(10, 0, 0), 2002 },
                    { 4004, 305, 504, 4, new TimeOnly(14, 30, 0), "c", new TimeOnly(13, 0, 0), 2003 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_ClassroomId",
                table: "ClassSchedules",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_CourseId",
                table: "ClassSchedules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_TeacherId",
                table: "ClassSchedules",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
