using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignment_Courses_CourseId",
                table: "TeacherAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignment_LevelTermSections_LevelTermSectionId",
                table: "TeacherAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignment_Sessionals_SessionalId",
                table: "TeacherAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_TeacherAssignment_TeacherAssignmentId",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherAssignment",
                table: "TeacherAssignment");

            migrationBuilder.RenameTable(
                name: "TeacherAssignment",
                newName: "TeacherAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAssignment_SessionalId",
                table: "TeacherAssignments",
                newName: "IX_TeacherAssignments_SessionalId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAssignment_LevelTermSectionId",
                table: "TeacherAssignments",
                newName: "IX_TeacherAssignments_LevelTermSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAssignment_CourseId",
                table: "TeacherAssignments",
                newName: "IX_TeacherAssignments_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherAssignments",
                table: "TeacherAssignments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_LevelTermSections_LevelTermSectionId",
                table: "TeacherAssignments",
                column: "LevelTermSectionId",
                principalTable: "LevelTermSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Sessionals_SessionalId",
                table: "TeacherAssignments",
                column: "SessionalId",
                principalTable: "Sessionals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_TeacherAssignments_TeacherAssignmentId",
                table: "Teachers",
                column: "TeacherAssignmentId",
                principalTable: "TeacherAssignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_LevelTermSections_LevelTermSectionId",
                table: "TeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Sessionals_SessionalId",
                table: "TeacherAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_TeacherAssignments_TeacherAssignmentId",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherAssignments",
                table: "TeacherAssignments");

            migrationBuilder.RenameTable(
                name: "TeacherAssignments",
                newName: "TeacherAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAssignments_SessionalId",
                table: "TeacherAssignment",
                newName: "IX_TeacherAssignment_SessionalId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAssignments_LevelTermSectionId",
                table: "TeacherAssignment",
                newName: "IX_TeacherAssignment_LevelTermSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherAssignments_CourseId",
                table: "TeacherAssignment",
                newName: "IX_TeacherAssignment_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherAssignment",
                table: "TeacherAssignment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignment_Courses_CourseId",
                table: "TeacherAssignment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignment_LevelTermSections_LevelTermSectionId",
                table: "TeacherAssignment",
                column: "LevelTermSectionId",
                principalTable: "LevelTermSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignment_Sessionals_SessionalId",
                table: "TeacherAssignment",
                column: "SessionalId",
                principalTable: "Sessionals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_TeacherAssignment_TeacherAssignmentId",
                table: "Teachers",
                column: "TeacherAssignmentId",
                principalTable: "TeacherAssignment",
                principalColumn: "Id");
        }
    }
}
