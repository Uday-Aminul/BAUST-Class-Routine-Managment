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
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Labrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Sessionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    Credit = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessionals_Teachers_TeacherId",
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
                    SessionalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    LabroomId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_ClassSchedules_Labrooms_LabroomId",
                        column: x => x.LabroomId,
                        principalTable: "Labrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabroomSessional",
                columns: table => new
                {
                    LabroomsId = table.Column<int>(type: "int", nullable: false),
                    SessionalsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabroomSessional", x => new { x.LabroomsId, x.SessionalsId });
                    table.ForeignKey(
                        name: "FK_LabroomSessional_Labrooms_LabroomsId",
                        column: x => x.LabroomsId,
                        principalTable: "Labrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabroomSessional_Sessionals_SessionalsId",
                        column: x => x.SessionalsId,
                        principalTable: "Sessionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                column: "Id",
                values: new object[]
                {
                    304,
                    305,
                    306,
                    308,
                    309,
                    310
                });

            migrationBuilder.InsertData(
                table: "Labrooms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 302, "Computer Lab A" },
                    { 307, "Computer Lab B" },
                    { 311, "Computer Lab c" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Code", "Designation", "Name" },
                values: new object[,]
                {
                    { 101, null, "Associate Professor", "A" },
                    { 102, null, "Associate Professor", "B" },
                    { 103, null, "Associate Professor", "C" },
                    { 104, null, "Associate Professor", "E" },
                    { 105, null, "Lecturer", "F" },
                    { 106, null, "Lecturer", "G" },
                    { 107, null, "Lecturer", "H" },
                    { 108, null, "Lecturer", "I" },
                    { 109, null, "Lecturer", "J" },
                    { 110, null, "Lecturer", "K" },
                    { 111, null, "Lecturer", "L" },
                    { 112, null, "Lecturer", "M" },
                    { 113, null, "Lecturer", "N" },
                    { 114, null, "Lecturer", "O" },
                    { 115, null, "Lecturer", "P" },
                    { 116, null, "Lecturer", "Q" },
                    { 117, null, "Lecturer", "R" },
                    { 118, null, "Lecturer", "S" },
                    { 119, null, "Lecturer", "T" },
                    { 120, null, "Lecturer", "U" },
                    { 121, null, "Lecturer", "V" },
                    { 122, null, "Lecturer", "W" },
                    { 123, null, "Lecturer", "X" },
                    { 124, null, "Lecturer", "Y" },
                    { 125, null, "Lecturer", "Z" },
                    { 126, null, "Assistant Lecturer", "AA" },
                    { 127, null, "Assistant Lecturer", "BB" },
                    { 128, null, "Assistant Lecturer", "CC" },
                    { 129, null, "Assistant Lecturer", "DD" },
                    { 130, null, "Assistant Lecturer", "EE" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "Credit", "Level", "Name", "TeacherId", "Term" },
                values: new object[,]
                {
                    { 501, null, 3, 3, "Data Structures", 101, 1 },
                    { 502, null, 3, 3, "Algorithms", 102, 2 },
                    { 503, null, 3, 3, "Circuit Analysis", 103, 1 },
                    { 504, null, 3, 3, "Electromagnetics", 104, 2 }
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
                name: "IX_ClassSchedules_LabroomId",
                table: "ClassSchedules",
                column: "LabroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_TeacherId",
                table: "ClassSchedules",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LabroomSessional_SessionalsId",
                table: "LabroomSessional",
                column: "SessionalsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessionals_TeacherId",
                table: "Sessionals",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.DropTable(
                name: "LabroomSessional");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Labrooms");

            migrationBuilder.DropTable(
                name: "Sessionals");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
