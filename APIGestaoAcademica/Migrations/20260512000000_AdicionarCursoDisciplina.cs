using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoAcademica.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCursoDisciplina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CursoId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CursoId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CursoId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CursoId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 5,
                column: "CursoId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 6,
                column: "CursoId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 7,
                column: "CursoId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 8,
                column: "CursoId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 9,
                column: "CursoId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 10,
                column: "CursoId",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_CursoId",
                table: "Disciplinas",
                column: "CursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Cursos_CursoId",
                table: "Disciplinas",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Cursos_CursoId",
                table: "Disciplinas");

            migrationBuilder.DropIndex(
                name: "IX_Disciplinas_CursoId",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Disciplinas");
        }
    }
}
