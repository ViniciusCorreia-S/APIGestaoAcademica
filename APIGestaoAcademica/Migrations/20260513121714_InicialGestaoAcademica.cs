using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIGestaoAcademica.Migrations
{
    /// <inheritdoc />
    public partial class InicialGestaoAcademica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DuracaoSemestres = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Matricula = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alunos_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CargaHoraria = table.Column<int>(type: "int", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplinas_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MatriculasDisciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<int>(type: "int", nullable: false),
                    DataMatricula = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatriculasDisciplinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatriculasDisciplinas_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatriculasDisciplinas_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Cursos",
                columns: new[] { "Id", "Codigo", "DuracaoSemestres", "Nome" },
                values: new object[,]
                {
                    { 1, "ADS", 5, "Analise e Desenvolvimento de Sistemas" },
                    { 2, "SI", 8, "Sistemas de Informacao" },
                    { 3, "CC", 8, "Ciencia da Computacao" },
                    { 4, "ES", 8, "Engenharia de Software" },
                    { 5, "SEG", 6, "Seguranca da Informacao" }
                });

            migrationBuilder.InsertData(
                table: "Alunos",
                columns: new[] { "Id", "Ativo", "CursoId", "DataNascimento", "Email", "Matricula", "Nome" },
                values: new object[,]
                {
                    { 1, true, 1, new DateOnly(2008, 3, 17), "viniciuscorreia@academico.local", "20260002", "Vinicius Correia" },
                    { 2, false, 2, new DateOnly(2002, 4, 12), "ana.souza@academico.local", "20260001", "Ana Souza" },
                    { 3, true, 3, new DateOnly(2004, 8, 25), "mariana.lima@academico.local", "20260003", "Mariana Lima" },
                    { 4, true, 4, new DateOnly(2003, 11, 9), "carlos.pereira@academico.local", "20260004", "Carlos Pereira" }
                });

            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "CargaHoraria", "Codigo", "CursoId", "Nome" },
                values: new object[,]
                {
                    { 1, 80, "ADS101", 1, "Algoritmos e Logica de Programacao" },
                    { 2, 80, "ADS102", 1, "Estrutura de Dados" },
                    { 3, 80, "SI101", 2, "Analise de Sistemas" },
                    { 4, 80, "SI102", 2, "Gestao de Projetos" },
                    { 5, 80, "CC101", 3, "Calculo I" },
                    { 6, 80, "CC102", 3, "Teoria da Computacao" },
                    { 7, 80, "ES101", 4, "Arquitetura de Software" },
                    { 8, 80, "ES102", 4, "Testes de Software" },
                    { 9, 80, "SEG101", 5, "Criptografia" },
                    { 10, 80, "SEG102", 5, "Seguranca de Redes" }
                });

            migrationBuilder.InsertData(
                table: "MatriculasDisciplinas",
                columns: new[] { "Id", "AlunoId", "DataMatricula", "DisciplinaId", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), 1, "Cursando" },
                    { 2, 1, new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), 2, "Cursando" },
                    { 3, 3, new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), 5, "Cursando" },
                    { 4, 3, new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), 6, "Cursando" },
                    { 5, 4, new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), 7, "Cursando" },
                    { 6, 4, new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), 8, "Cursando" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_CursoId",
                table: "Alunos",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Email",
                table: "Alunos",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Matricula",
                table: "Alunos",
                column: "Matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_Codigo",
                table: "Cursos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_Codigo",
                table: "Disciplinas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_CursoId",
                table: "Disciplinas",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_MatriculasDisciplinas_AlunoId_DisciplinaId",
                table: "MatriculasDisciplinas",
                columns: new[] { "AlunoId", "DisciplinaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatriculasDisciplinas_DisciplinaId",
                table: "MatriculasDisciplinas",
                column: "DisciplinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatriculasDisciplinas");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Cursos");
        }
    }
}
