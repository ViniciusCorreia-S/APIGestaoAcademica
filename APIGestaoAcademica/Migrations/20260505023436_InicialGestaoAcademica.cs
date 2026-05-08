using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestaoAcademica.Migrations
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
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CargaHoraria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
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
	            table: "Disciplinas",
	            columns: new[] { "Id", "CargaHoraria", "Codigo", "Nome" },
	            values: new object[,]
	            {
                    // ===== ADS (CursoId = 1) =====
                    { 1, 80, "ADS101", "Algoritmos e Logica de Programacao" },
		            { 2, 80, "ADS102", "Estrutura de Dados" },

                    // ===== Sistemas de Informacao (CursoId = 2) =====
                   
		            { 3, 80, "SI101", "Analise de Sistemas" },
		            { 4, 80, "SI102", "Gestao de Projetos" },

                    // ===== Ciencia da Computacao (CursoId = 3) =====
                    { 5, 80, "CC101", "Calculo I" },
		            { 6, 80, "CC102", "Teoria da Computacao" },
                    // ===== Engenharia de Software (CursoId = 4) =====
                   
		            { 7, 80, "ES101", "Arquitetura de Software" },
		            { 8, 80, "ES102", "Testes de Software" },

                    // ===== Seguranca da Informacao (CursoId = 5) =====
                    { 9, 80, "SEG101", "Criptografia" },
		            { 10, 80, "SEG102", "Seguranca de Redes" },
	            });

			migrationBuilder.InsertData(
                table: "Alunos",
                columns: new[] { "Id", "Ativo", "CursoId", "DataNascimento", "Email", "Matricula", "Nome" },
                values: new object[,] 
                { 
                    { 1, false, 2, new DateOnly(2002, 4, 12), "ana.souza@academico.local", "20260001", "Ana Souza" },
                    { 2, true, 1, new DateOnly(2008, 3, 17), "viniciuscorreia@academico.local", "20260002", "Vinicius Correia"}
                }
			);

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
