using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
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
                name: "LevelTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelTerms_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Credit = table.Column<float>(type: "real", nullable: false),
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
                    Credit = table.Column<float>(type: "real", nullable: false),
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
                    ClassroomId = table.Column<int>(type: "int", nullable: true),
                    LabroomId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    SessionalId = table.Column<int>(type: "int", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Labrooms_LabroomId",
                        column: x => x.LabroomId,
                        principalTable: "Labrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassSchedules_Sessionals_SessionalId",
                        column: x => x.SessionalId,
                        principalTable: "Sessionals",
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
                    AllowedSessionalsId = table.Column<int>(type: "int", nullable: false),
                    LabroomsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabroomSessional", x => new { x.AllowedSessionalsId, x.LabroomsId });
                    table.ForeignKey(
                        name: "FK_LabroomSessional_Labrooms_LabroomsId",
                        column: x => x.LabroomsId,
                        principalTable: "Labrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabroomSessional_Sessionals_AllowedSessionalsId",
                        column: x => x.AllowedSessionalsId,
                        principalTable: "Sessionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                column: "Id",
                values: new object[]
                {
                    204,
                    205,
                    304,
                    305,
                    306,
                    308,
                    309,
                    310,
                    311,
                    407,
                    408,
                    1001
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "Credit", "Level", "Name", "TeacherId", "Term" },
                values: new object[] { 5, "MATH 1141", 3f, 1, "Differential Calculus, Integral Calculus, and Coordinate Geometry", null, 1 });

            migrationBuilder.InsertData(
                table: "Labrooms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 202, "EEE" },
                    { 210, "CSE" },
                    { 302, "CSE" },
                    { 307, "CSE" },
                    { 311, "CSE" },
                    { 402, "CSE, CAD" },
                    { 411, "CSE" },
                    { 1001, "AC Circuit Lab" },
                    { 1002, "DC Circuit Lab" },
                    { 1003, "AC Circuit Lab" },
                    { 1004, "Electronics Lab" },
                    { 1005, "Physics Lab" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Code", "Designation", "Name" },
                values: new object[,]
                {
                    { 1, "TQA", "Lecturer", "TQA" },
                    { 2, "NHC", "Lecturer", "NHC" },
                    { 3, "PHY", "Assistant Professor", "PHY" },
                    { 4, "MAM", "Assistant Professor", "MAM" },
                    { 5, "ENG1", "Lecturer", "ENG1" },
                    { 6, "EEE1", "Lecturer", "EEE1" },
                    { 7, "MA", "Lecturer", "MA" },
                    { 8, "AH", "Assistant Professor", "AH" },
                    { 9, "MH", "Lecturer", "MH" },
                    { 10, "EMH", "Lecturer", "EMH" },
                    { 11, "MATH2", "Assistant Professor", "MATH2" },
                    { 12, "SG", "Lecturer", "SG" },
                    { 13, "MSZ", "Lecturer", "MSZ" },
                    { 14, "AHS", "Lecturer", "AHS" },
                    { 15, "ASM", "Lecturer", "ASM" },
                    { 16, "MSA", "Assistant Professor", "MSA" },
                    { 17, "CHEM", "Assistant Professor", "CHEM" },
                    { 18, "MATH3", "Professor", "MATH3" },
                    { 19, "MZH", "Assistant Professor", "MZH" },
                    { 20, "MO", "Lecturer", "MO" },
                    { 21, "RR", "Assistant Professor", "RR" },
                    { 22, "MATH4", "Professor", "MATH4" },
                    { 23, "ST", "Assistant Professor", "ST" },
                    { 24, "AKZ", "Lecturer", "AKZ" },
                    { 25, "GR", "Assistant Professor", "GR" },
                    { 26, "EAS", "Lecturer", "EAS" },
                    { 27, "EEE2", "Lecturer", "EEE2" },
                    { 28, "MI", "Assistant Professor", "MI" },
                    { 29, "TMM", "Lecturer", "TMM" },
                    { 30, "NAO", "Lecturer", "NAO" },
                    { 31, "NR", "Assistant Professor", "NR" },
                    { 32, "AS", "Assistant Professor", "AS" },
                    { 33, "JA", "Lecturer", "JA" },
                    { 34, "AA", "Lecturer", "AA" },
                    { 35, "AZ", "Lecturer", "AZ" },
                    { 36, "BBA", "Lecturer", "BBA" },
                    { 37, "SA", "Lecturer", "SA" },
                    { 38, "NF1", "Lecturer", "NF1" },
                    { 39, "IPE", "Assistant Professor", "IPE" },
                    { 40, "AIS", "Lecturer", "AIS" },
                    { 41, "RA", "Lecturer", "RA" },
                    { 42, "MAS", "Lecturer", "MAS" },
                    { 43, "PR", "Lecturer", "PR" },
                    { 44, "ENG2", "Lecturer", "ENG2" },
                    { 45, "HUM1", "Lecturer", "HUM1" },
                    { 46, "HUM2", "Lecturer", "HUM2" },
                    { 47, "ME", "Assistant Professor", "ME" },
                    { 48, "NHS", "Lecturer", "NHS" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "Credit", "Level", "Name", "TeacherId", "Term" },
                values: new object[,]
                {
                    { 1, "EEE 1163", 3f, 1, "Introduction to Electrical Engineering", 1, 1 },
                    { 2, "CSE 1101", 3f, 1, "Structured Programming Language", 2, 1 },
                    { 3, "PHY 1131", 3f, 1, "Physics", 3, 1 },
                    { 4, "ENG 1127", 3f, 1, "English", 5, 1 },
                    { 6, "CSE 1203", 3f, 1, "Object Oriented Programming Language I", 8, 2 },
                    { 7, "EEE 1269", 3f, 1, "Electronic Circuits", 10, 2 },
                    { 8, "MATH 1243", 3f, 1, "Ordinary Differential Equations and Partial Differential Equations", 11, 2 },
                    { 9, "CSE 1201", 3f, 1, "Discrete Mathematics", 14, 2 },
                    { 10, "HUM 1221", 2f, 1, "Bengali Language and Literature", 45, 2 },
                    { 11, "CSE 2101", 3f, 2, "Digital Logic Design", 16, 1 },
                    { 12, "CHEM 2133", 3f, 2, "Chemistry", 17, 1 },
                    { 13, "MATH 2145", 3f, 2, "Vector Calculus, Linear Algebra and Complex Variable", 18, 1 },
                    { 14, "CSE 2103", 3f, 2, "Data Structures and Algorithm I", 19, 1 },
                    { 15, "CSE 2105", 3f, 2, "Applied Statistics for Computer Science", 20, 1 },
                    { 16, "CSE 2201", 3f, 2, "Data Structures and Algorithm II", 21, 2 },
                    { 17, "MATH 2247", 3f, 2, "Laplace Transformation and Fourier Analysis", 22, 2 },
                    { 18, "CSE 2203", 3f, 2, "Theory of Computation", 23, 2 },
                    { 19, "EEE 2269", 3f, 2, "Electrical Drives and Instrumentation", 26, 2 },
                    { 20, "CSE 2205", 3f, 2, "Database Management Systems", 24, 2 },
                    { 21, "HUM 2221", 2f, 2, "History of the Emergence of Bangladesh", 46, 2 },
                    { 22, "CSE 3109", 3f, 3, "Compiler", 28, 1 },
                    { 23, "CSE 3103", 3f, 3, "Microprocessors, Microcontrollers and Embedded Systems", 29, 1 },
                    { 24, "CSE 3107", 3f, 3, "Data Communication", 4, 1 },
                    { 25, "CSE 3101", 3f, 3, "Software Engineering", 12, 1 },
                    { 26, "ME 3181", 3f, 3, "Basic Mechanical Engineering", 47, 1 },
                    { 27, "CSE 3105", 3f, 3, "Computer Architecture", 9, 1 },
                    { 28, "CSE 3201", 3f, 3, "Artificial Intelligence", 32, 2 },
                    { 29, "CSE 3203", 3f, 3, "Operating System", 30, 2 },
                    { 30, "CSE 3207", 3f, 3, "Mathematical Analysis for Computer Science", 25, 2 },
                    { 31, "CSE 3205", 3f, 3, "Computer Networks", 31, 2 },
                    { 32, "CSE 3209", 3f, 3, "Information System Design", 13, 2 },
                    { 33, "IPE 4217", 2f, 3, "Industrial Management", 39, 2 },
                    { 34, "CSE 4139", 3f, 4, "Machine Learning", 33, 1 },
                    { 35, "CSE 4103", 3f, 4, "Computer Graphics", 34, 1 },
                    { 36, "HUM 4123", 3f, 4, "Engineering Economics", 36, 1 },
                    { 37, "CSE 4141", 3f, 4, "Object Oriented Software Engineering", 8, 1 },
                    { 38, "CSE 4101", 3f, 4, "Computer Security", 35, 1 },
                    { 39, "CSE 4251", 2f, 4, "Data Warehousing and Data Mining", 31, 2 },
                    { 40, "CSE 4245", 2f, 4, "Digital Image Processing", 33, 2 },
                    { 41, "CSE 4215", 2f, 4, "Professional Issues and Ethics in Computer Science", 38, 2 },
                    { 42, "HUM 4273", 2f, 4, "Financial, Cost and Managerial Accounting", 40, 2 },
                    { 43, "CSE 4249", 2f, 4, "VLSI Design", 41, 2 }
                });

            migrationBuilder.InsertData(
                table: "LevelTerms",
                columns: new[] { "Id", "ClassroomId", "Level", "Term" },
                values: new object[,]
                {
                    { 1, 408, 1, 1 },
                    { 2, 308, 1, 2 },
                    { 3, 305, 2, 1 },
                    { 4, 309, 2, 2 },
                    { 5, 304, 3, 1 },
                    { 6, 204, 3, 2 },
                    { 7, 306, 4, 1 },
                    { 8, 407, 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "Sessionals",
                columns: new[] { "Id", "Credit", "Level", "Name", "SessionalCode", "TeacherId", "Term" },
                values: new object[,]
                {
                    { 1, 1.5f, 1, "Introduction to Computer System Sessional", "CSE 1100", 9, 1 },
                    { 2, 1.5f, 1, "Structured Programming Language Sessional", "CSE 1102", 2, 1 },
                    { 3, 1.5f, 1, "Introduction to Electrical Engineering Sessional", "EEE 1164", 7, 1 },
                    { 4, 0.75f, 1, "Physics Sessional", "PHY 1132", 3, 1 },
                    { 5, 1.5f, 1, "Object Oriented Programming Language I Sessional", "CSE 1204", 8, 2 },
                    { 6, 1.5f, 1, "Numerical Methods Sessional", "CSE 1208", 12, 2 },
                    { 7, 0.75f, 1, "Electronic Circuits Sessional", "EEE 1270", 26, 2 },
                    { 8, 0.75f, 1, "Engineering Drawing and CAD Sessional", "CE 1250", 43, 2 },
                    { 9, 0.75f, 1, "Developing English Skill Sessional", "ENG 1228", 44, 2 },
                    { 10, 1.5f, 2, "Digital Logic Design Sessional", "CSE 2102", 37, 1 },
                    { 11, 1.5f, 2, "Data Structures and Algorithm I Sessional", "CSE 2104", 19, 1 },
                    { 12, 1.5f, 2, "Object Oriented Programming Language II Sessional", "CSE 2108", 34, 1 },
                    { 13, 0.75f, 2, "Software Development Project I", "CSE 2100", 4, 1 },
                    { 14, 1.5f, 2, "Data Structures and Algorithm II Sessional", "CSE 2202", 20, 2 },
                    { 15, 1.5f, 2, "Database Management Systems Sessional", "CSE 2206", 24, 2 },
                    { 16, 0.75f, 2, "Electrical Drives and Instrumentation Sessional", "EEE 2270", 27, 2 },
                    { 17, 0.75f, 3, "Software Engineering Sessional", "CSE 3102", 12, 1 },
                    { 18, 0.75f, 3, "Microprocessors, Microcontrollers and Embedded Systems Sessional", "CSE 3104", 29, 1 },
                    { 19, 0.75f, 3, "Compiler Sessional", "CSE 3110", 28, 1 },
                    { 20, 0.75f, 3, "Software Development Project II", "CSE 3100", 4, 1 },
                    { 21, 0.75f, 3, "Artificial Intelligence Sessional", "CSE 3202", 32, 2 },
                    { 22, 1.5f, 3, "Operating System Sessional", "CSE 3204", 37, 2 },
                    { 23, 1.5f, 3, "Computer Networks Sessional", "CSE 3206", 28, 2 },
                    { 24, 0.75f, 3, "Information System Design Sessional", "CSE 3210", 32, 2 },
                    { 25, 1.5f, 3, "Web Engineering Project", "CSE 3200", 23, 2 },
                    { 26, 0.75f, 4, "Computer Security Sessional", "CSE 4102", 35, 1 },
                    { 27, 0.75f, 4, "Computer Graphics Sessional", "CSE 4104", 29, 1 },
                    { 28, 0.75f, 4, "Machine Learning Sessional", "CSE 4140", 16, 1 },
                    { 29, 0.75f, 4, "Object Oriented Software Engineering Sessional", "CSE 4142", 8, 1 },
                    { 30, 0.75f, 4, "Digital Image Processing Sessional", "CSE 4246", 33, 2 },
                    { 31, 0.75f, 4, "Data Warehousing and Data Mining Sessional", "CSE 4252", 31, 2 }
                });

            migrationBuilder.InsertData(
                table: "ClassSchedules",
                columns: new[] { "Id", "ClassroomId", "CourseId", "Day", "EndTime", "LabroomId", "SessionalId", "StartTime", "TeacherId" },
                values: new object[,]
                {
                    { 1, 407, 33, 1, new TimeOnly(10, 50, 0), null, null, new TimeOnly(10, 0, 0), 39 },
                    { 2, 407, 39, 1, new TimeOnly(12, 20, 0), null, null, new TimeOnly(11, 30, 0), 31 },
                    { 3, 407, 41, 1, new TimeOnly(13, 20, 0), null, null, new TimeOnly(12, 30, 0), 38 },
                    { 4, 407, 41, 2, new TimeOnly(8, 50, 0), null, null, new TimeOnly(8, 0, 0), 38 },
                    { 5, 407, 39, 2, new TimeOnly(9, 50, 0), null, null, new TimeOnly(9, 0, 0), 31 },
                    { 6, 407, 40, 2, new TimeOnly(10, 50, 0), null, null, new TimeOnly(10, 0, 0), 33 },
                    { 7, 407, 33, 2, new TimeOnly(12, 20, 0), null, null, new TimeOnly(11, 30, 0), 39 },
                    { 8, 407, 42, 3, new TimeOnly(12, 20, 0), null, null, new TimeOnly(11, 30, 0), 40 },
                    { 9, 407, 40, 3, new TimeOnly(13, 20, 0), null, null, new TimeOnly(12, 30, 0), 33 },
                    { 10, 407, 33, 3, new TimeOnly(14, 20, 0), null, null, new TimeOnly(13, 30, 0), 39 },
                    { 11, 407, 40, 4, new TimeOnly(8, 50, 0), null, null, new TimeOnly(8, 0, 0), 33 },
                    { 12, 407, 39, 4, new TimeOnly(9, 50, 0), null, null, new TimeOnly(9, 0, 0), 31 },
                    { 13, 407, 42, 4, new TimeOnly(10, 50, 0), null, null, new TimeOnly(10, 0, 0), 40 },
                    { 14, 407, 43, 4, new TimeOnly(12, 20, 0), null, null, new TimeOnly(11, 30, 0), 41 },
                    { 15, null, null, 4, new TimeOnly(3, 20, 0), 411, 30, new TimeOnly(2, 30, 0), 33 },
                    { 16, null, null, 0, new TimeOnly(8, 50, 0), 307, 31, new TimeOnly(8, 0, 0), 31 }
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
                name: "IX_ClassSchedules_SessionalId",
                table: "ClassSchedules",
                column: "SessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedules_TeacherId",
                table: "ClassSchedules",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LabroomSessional_LabroomsId",
                table: "LabroomSessional",
                column: "LabroomsId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelTerms_ClassroomId",
                table: "LevelTerms",
                column: "ClassroomId",
                unique: true);

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
                name: "LevelTerms");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Labrooms");

            migrationBuilder.DropTable(
                name: "Sessionals");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
