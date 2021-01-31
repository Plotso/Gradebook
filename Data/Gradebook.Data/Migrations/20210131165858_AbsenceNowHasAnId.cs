using Microsoft.EntityFrameworkCore.Migrations;

namespace Gradebook.Data.Migrations
{
    public partial class AbsenceNowHasAnId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentSubjectSubjectId",
                table: "Absences",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentSubjectStudentId",
                table: "Absences",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentSubjectId",
                table: "Absences",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentSubjectId",
                table: "Absences");

            migrationBuilder.AlterColumn<int>(
                name: "StudentSubjectSubjectId",
                table: "Absences",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "StudentSubjectStudentId",
                table: "Absences",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
