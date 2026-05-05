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
                    RoomNumber = table.Column<int>(type: "int", nullable: false)
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
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LevelTermSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelTermSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomLevelTermSection",
                columns: table => new
                {
                    ClassroomsId = table.Column<int>(type: "int", nullable: false),
                    LevelTermSectionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomLevelTermSection", x => new { x.ClassroomsId, x.LevelTermSectionsId });
                    table.ForeignKey(
                        name: "FK_ClassroomLevelTermSection_Classrooms_ClassroomsId",
                        column: x => x.ClassroomsId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomLevelTermSection_LevelTermSections_LevelTermSectionsId",
                        column: x => x.LevelTermSectionsId,
                        principalTable: "LevelTermSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassroomId = table.Column<int>(type: "int", nullable: true),
                    LabroomId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    SessionalId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_ClassSchedules_Labrooms_LabroomId",
                        column: x => x.LabroomId,
                        principalTable: "Labrooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassScheduleTeacher",
                columns: table => new
                {
                    ClassesId = table.Column<int>(type: "int", nullable: false),
                    TeachersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassScheduleTeacher", x => new { x.ClassesId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_ClassScheduleTeacher_ClassSchedules_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "ClassSchedules",
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
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<int>(type: "int", nullable: false),
                    Credit = table.Column<float>(type: "real", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
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
                });

            migrationBuilder.CreateTable(
                name: "TeacherAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelTermSectionId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    SessionalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherAssignment_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherAssignment_LevelTermSections_LevelTermSectionId",
                        column: x => x.LevelTermSectionId,
                        principalTable: "LevelTermSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherAssignment_Sessionals_SessionalId",
                        column: x => x.SessionalId,
                        principalTable: "Sessionals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherAssignmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_TeacherAssignment_TeacherAssignmentId",
                        column: x => x.TeacherAssignmentId,
                        principalTable: "TeacherAssignment",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "RoomNumber" },
                values: new object[,]
                {
                    { 1, 204 },
                    { 2, 205 },
                    { 3, 304 },
                    { 4, 305 },
                    { 5, 306 },
                    { 6, 308 },
                    { 7, 309 },
                    { 8, 310 },
                    { 9, 311 },
                    { 10, 407 },
                    { 11, 408 },
                    { 12, 402 },
                    { 13, 502 },
                    { 14, 506 },
                    { 15, 507 },
                    { 16, 510 },
                    { 17, 1001 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "Credit", "Level", "Name", "TeacherId", "Term" },
                values: new object[,]
                {
                    { 1, "EEE 1163", 3f, 1, "Introduction to Electrical Engineering", null, 1 },
                    { 2, "CSE 1101", 3f, 1, "Structured Programming Language", null, 1 },
                    { 3, "PHY 1131", 3f, 1, "Physics", null, 1 },
                    { 4, "ENG 1127", 3f, 1, "English", null, 1 },
                    { 5, "MATH 1141", 3f, 1, "MATH 1141", null, 1 },
                    { 6, "CSE 1203", 3f, 1, "Object Oriented Programming Language I", null, 2 },
                    { 7, "EEE 1269", 3f, 1, "Electronic Circuits", null, 2 },
                    { 8, "MATH 1243", 3f, 1, "MATH 1243", null, 2 },
                    { 9, "CSE 1201", 3f, 1, "Discrete Mathematics", null, 2 },
                    { 10, "HUM 1221", 2f, 1, "Bengali Language and Literature", null, 2 },
                    { 11, "CSE 2101", 3f, 2, "Digital Logic Design", null, 1 },
                    { 12, "CHEM 2133", 3f, 2, "Chemistry", null, 1 },
                    { 13, "MATH 2145", 3f, 2, "MATH 2145", null, 1 },
                    { 14, "CSE 2103", 3f, 2, "Data Structures and Algorithm I", null, 1 },
                    { 15, "CSE 2105", 3f, 2, "Applied Statistics", null, 1 },
                    { 16, "CSE 2201", 3f, 2, "Data Structures and Algorithm II", null, 2 },
                    { 17, "MATH 2247", 3f, 2, "MATH 2247", null, 2 },
                    { 18, "CSE 2203", 3f, 2, "Theory of Computation", null, 2 },
                    { 19, "EEE 2269", 3f, 2, "Electrical Drives", null, 2 },
                    { 20, "CSE 2205", 3f, 2, "Database Management Systems", null, 2 },
                    { 21, "HUM 2221", 2f, 2, "History of Bangladesh", null, 2 },
                    { 22, "CSE 3109", 3f, 3, "Compiler", null, 1 },
                    { 23, "CSE 3103", 3f, 3, "Microprocessors", null, 1 },
                    { 24, "CSE 3107", 3f, 3, "Data Communication", null, 1 },
                    { 25, "CSE 3101", 3f, 3, "Software Engineering", null, 1 },
                    { 26, "ME 3181", 3f, 3, "Mechanical Engineering", null, 1 },
                    { 27, "CSE 3105", 3f, 3, "Computer Architecture", null, 1 },
                    { 28, "CSE 3201", 3f, 3, "Artificial Intelligence", null, 2 },
                    { 29, "CSE 3203", 3f, 3, "Operating System", null, 2 },
                    { 30, "CSE 3207", 3f, 3, "Math Analysis", null, 2 },
                    { 31, "CSE 3205", 3f, 3, "Computer Networks", null, 2 },
                    { 32, "CSE 3209", 3f, 3, "Information System Design", null, 2 },
                    { 33, "CSE 4139", 3f, 4, "Machine Learning", null, 1 },
                    { 34, "CSE 4103", 3f, 4, "Computer Graphics", null, 1 },
                    { 35, "HUM 4123", 3f, 4, "Engineering Economics", null, 1 },
                    { 36, "CSE 4141", 3f, 4, "Object Oriented Software Eng", null, 1 },
                    { 37, "CSE 4101", 3f, 4, "Computer Security", null, 1 },
                    { 38, "IPE 4217", 2f, 4, "Industrial Management", null, 2 },
                    { 39, "CSE 4251", 2f, 4, "Data Warehousing", null, 2 },
                    { 40, "CSE 4245", 2f, 4, "Image Processing", null, 2 },
                    { 41, "CSE 4215", 2f, 4, "Professional Ethics", null, 2 },
                    { 42, "HUM 4273", 2f, 4, "Accounting", null, 2 },
                    { 43, "CSE 4249", 2f, 4, "VLSI Design", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Labrooms",
                columns: new[] { "Id", "Name", "RoomNumber" },
                values: new object[,]
                {
                    { 1, "Room 014", 14 },
                    { 2, "Room 024", 24 },
                    { 3, "Room 107", 107 },
                    { 4, "Room 108", 108 },
                    { 5, "Room 110", 110 },
                    { 6, "EEE Lab", 202 },
                    { 7, "Room 206", 206 },
                    { 8, "CSE Lab 210", 210 },
                    { 9, "CSE Lab 302", 302 },
                    { 10, "CSE Lab 307", 307 },
                    { 11, "CSE Lab 311", 311 },
                    { 12, "CSE/CAD Lab 402", 402 },
                    { 13, "CSE Lab 411", 411 },
                    { 14, "AC Circuit Lab", 1001 },
                    { 15, "DC Circuit Lab", 1002 },
                    { 16, "AC Circuit Lab", 1003 },
                    { 17, "Electronics Lab", 1004 },
                    { 18, "Physics Lab", 1005 },
                    { 19, "IPE Computer Lab", 109 }
                });

            migrationBuilder.InsertData(
                table: "LevelTermSections",
                columns: new[] { "Id", "Level", "Section", "Term" },
                values: new object[,]
                {
                    { 1, 1, "A", 1 },
                    { 2, 1, "B", 1 },
                    { 3, 1, "A", 2 },
                    { 4, 1, "B", 2 },
                    { 5, 2, "A", 1 },
                    { 6, 2, "B", 1 },
                    { 7, 2, "A", 2 },
                    { 8, 2, "B", 2 },
                    { 10, 3, "A", 1 },
                    { 11, 3, "B", 1 },
                    { 12, 3, "A", 2 },
                    { 13, 3, "B", 2 },
                    { 14, 4, "A", 1 },
                    { 15, 4, "B", 1 },
                    { 16, 4, "A", 2 },
                    { 17, 4, "B", 2 }
                });

            migrationBuilder.InsertData(
                table: "Sessionals",
                columns: new[] { "Id", "Credit", "Level", "Name", "SessionalCode", "TeacherId", "Term" },
                values: new object[,]
                {
                    { 1, 1.5f, 1, "Intro to Computer System Sessional", "CSE 1100", null, 1 },
                    { 2, 1.5f, 1, "Structured Programming Language Sessional", "CSE 1102", null, 1 },
                    { 3, 1.5f, 1, "Intro to Electrical Engineering Sessional", "EEE 1164", null, 1 },
                    { 4, 0.75f, 1, "Physics Sessional", "PHY 1132", null, 1 },
                    { 5, 1.5f, 1, "OOP I Sessional", "CSE 1204", null, 2 },
                    { 6, 1.5f, 1, "Numerical Methods Sessional", "CSE 1208", null, 2 },
                    { 7, 0.75f, 1, "Electronic Circuits Sessional", "EEE 1270", null, 2 },
                    { 8, 0.75f, 1, "Engineering Drawing Sessional", "CE 1250", null, 2 },
                    { 9, 0.75f, 1, "English Skill Sessional", "ENG 1228", null, 2 },
                    { 10, 1.5f, 2, "Digital Logic Design Sessional", "CSE 2102", null, 1 },
                    { 11, 1.5f, 2, "Data Structures I Sessional", "CSE 2104", null, 1 },
                    { 12, 1.5f, 2, "OOP II Sessional", "CSE 2108", null, 1 },
                    { 13, 0.75f, 2, "Software Development Project I", "CSE 2100", null, 1 },
                    { 14, 1.5f, 2, "Data Structures II Sessional", "CSE 2202", null, 2 },
                    { 15, 1.5f, 2, "Database Sessional", "CSE 2206", null, 2 },
                    { 16, 0.75f, 2, "Electrical Drives Sessional", "EEE 2270", null, 2 },
                    { 17, 0.75f, 3, "Software Engineering Sessional", "CSE 3102", null, 1 },
                    { 18, 0.75f, 3, "Microprocessors Sessional", "CSE 3104", null, 1 },
                    { 19, 0.75f, 3, "Compiler Sessional", "CSE 3110", null, 1 },
                    { 20, 0.75f, 3, "Software Development Project II", "CSE 3100", null, 1 },
                    { 21, 0.75f, 3, "AI Sessional", "CSE 3202", null, 2 },
                    { 22, 1.5f, 3, "OS Sessional", "CSE 3204", null, 2 },
                    { 23, 1.5f, 3, "Networks Sessional", "CSE 3206", null, 2 },
                    { 24, 0.75f, 3, "Info System Design Sessional", "CSE 3210", null, 2 },
                    { 25, 1.5f, 3, "Web Engineering Project", "CSE 3200", null, 2 },
                    { 26, 0.75f, 4, "Computer Security Sessional", "CSE 4102", null, 1 },
                    { 27, 0.75f, 4, "Computer Graphics Sessional", "CSE 4104", null, 1 },
                    { 28, 0.75f, 4, "Machine Learning Sessional", "CSE 4140", null, 1 },
                    { 29, 0.75f, 4, "OOSE Sessional", "CSE 4142", null, 1 },
                    { 30, 0.75f, 4, "Image Processing Sessional", "CSE 4246", null, 2 },
                    { 31, 0.75f, 4, "Data Mining Sessional", "CSE 4252", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Code", "Designation", "Name", "TeacherAssignmentId" },
                values: new object[,]
                {
                    { 1, "TQA", "Lecturer", "TQA", null },
                    { 2, "NHC", "Lecturer", "NHC", null },
                    { 3, "PHY", "Assistant Professor", "PHY", null },
                    { 4, "MAM", "Assistant Professor", "MAM", null },
                    { 5, "ENG1", "Lecturer", "ENG1", null },
                    { 6, "EEE1", "Lecturer", "EEE1", null },
                    { 7, "MA", "Lecturer", "MA", null },
                    { 8, "AH", "Assistant Professor", "AH", null },
                    { 9, "MH", "Lecturer", "MH", null },
                    { 10, "EMH", "Lecturer", "EMH", null },
                    { 11, "MATH2", "Assistant Professor", "MATH2", null },
                    { 12, "SG", "Lecturer", "SG", null },
                    { 13, "MSZ", "Lecturer", "MSZ", null },
                    { 14, "AHS", "Lecturer", "AHS", null },
                    { 15, "ASM", "Lecturer", "ASM", null },
                    { 16, "MSA", "Assistant Professor", "MSA", null },
                    { 17, "CHEM", "Assistant Professor", "CHEM", null },
                    { 18, "MATH3", "Professor", "MATH3", null },
                    { 19, "MZH", "Assistant Professor", "MZH", null },
                    { 20, "MO", "Lecturer", "MO", null },
                    { 21, "RR", "Assistant Professor", "RR", null },
                    { 22, "MATH4", "Professor", "MATH4", null },
                    { 23, "ST", "Assistant Professor", "ST", null },
                    { 24, "AKZ", "Lecturer", "AKZ", null },
                    { 25, "GR", "Assistant Professor", "GR", null },
                    { 26, "EAS", "Lecturer", "EAS", null },
                    { 27, "EEE2", "Lecturer", "EEE2", null },
                    { 28, "MI", "Assistant Professor", "MI", null },
                    { 29, "TMM", "Lecturer", "TMM", null },
                    { 30, "NAO", "Lecturer", "NAO", null },
                    { 31, "NR", "Assistant Professor", "NR", null },
                    { 32, "AS", "Assistant Professor", "AS", null },
                    { 33, "JA", "Lecturer", "JA", null },
                    { 34, "AA", "Lecturer", "AA", null },
                    { 35, "AZ", "Lecturer", "AZ", null },
                    { 36, "BBA", "Lecturer", "BBA", null },
                    { 37, "SA", "Lecturer", "SA", null },
                    { 38, "NF1", "Lecturer", "NF1", null },
                    { 39, "IPE", "Assistant Professor", "IPE", null },
                    { 40, "AIS", "Lecturer", "AIS", null },
                    { 41, "RA", "Lecturer", "RA", null },
                    { 42, "MAS", "Lecturer", "MAS", null },
                    { 43, "PR", "Lecturer", "PR", null },
                    { 44, "ENG2", "Lecturer", "ENG2", null },
                    { 45, "HUM1", "Lecturer", "HUM1", null },
                    { 46, "HUM2", "Lecturer", "HUM2", null },
                    { 47, "ME", "Assistant Professor", "ME", null },
                    { 48, "NHS", "Lecturer", "NHS", null },
                    { 49, "MATH1", "Professor", "MATH1", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomLevelTermSection_LevelTermSectionsId",
                table: "ClassroomLevelTermSection",
                column: "LevelTermSectionsId");

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
                name: "IX_ClassScheduleTeacher_TeachersId",
                table: "ClassScheduleTeacher",
                column: "TeachersId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LabroomSessional_LabroomsId",
                table: "LabroomSessional",
                column: "LabroomsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessionals_TeacherId",
                table: "Sessionals",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAssignment_CourseId",
                table: "TeacherAssignment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAssignment_LevelTermSectionId",
                table: "TeacherAssignment",
                column: "LevelTermSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAssignment_SessionalId",
                table: "TeacherAssignment",
                column: "SessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherAssignmentId",
                table: "Teachers",
                column: "TeacherAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Courses_CourseId",
                table: "ClassSchedules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Sessionals_SessionalId",
                table: "ClassSchedules",
                column: "SessionalId",
                principalTable: "Sessionals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassScheduleTeacher_Teachers_TeachersId",
                table: "ClassScheduleTeacher",
                column: "TeachersId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabroomSessional_Sessionals_AllowedSessionalsId",
                table: "LabroomSessional",
                column: "AllowedSessionalsId",
                principalTable: "Sessionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessionals_Teachers_TeacherId",
                table: "Sessionals",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignment_LevelTermSections_LevelTermSectionId",
                table: "TeacherAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignment_Courses_CourseId",
                table: "TeacherAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignment_Sessionals_SessionalId",
                table: "TeacherAssignment");

            migrationBuilder.DropTable(
                name: "ClassroomLevelTermSection");

            migrationBuilder.DropTable(
                name: "ClassScheduleTeacher");

            migrationBuilder.DropTable(
                name: "LabroomSessional");

            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Labrooms");

            migrationBuilder.DropTable(
                name: "LevelTermSections");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Sessionals");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "TeacherAssignment");
        }
    }
}
