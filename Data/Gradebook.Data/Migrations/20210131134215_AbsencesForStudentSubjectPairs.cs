using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gradebook.Data.Migrations
{
    public partial class AbsencesForStudentSubjectPairs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Absences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Period = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    StudentSubjectStudentId = table.Column<int>(nullable: true),
                    StudentSubjectSubjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absences_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Absences_StudentSubjects_StudentSubjectStudentId_StudentSubjectSubjectId",
                        columns: x => new { x.StudentSubjectStudentId, x.StudentSubjectSubjectId },
                        principalTable: "StudentSubjects",
                        principalColumns: new[] { "StudentId", "SubjectId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absences_IsDeleted",
                table: "Absences",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_TeacherId",
                table: "Absences",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_StudentSubjectStudentId_StudentSubjectSubjectId",
                table: "Absences",
                columns: new[] { "StudentSubjectStudentId", "StudentSubjectSubjectId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absences");
        }
    }
}
