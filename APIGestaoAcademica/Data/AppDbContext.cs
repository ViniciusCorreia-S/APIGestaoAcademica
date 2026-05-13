using GestaoAcademica.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademica.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Disciplina> Disciplinas => Set<Disciplina>();
    public DbSet<MatriculaDisciplina> MatriculasDisciplinas => Set<MatriculaDisciplina>();

    // Configurações adicionais do modelo
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>()
            .HasIndex(aluno => aluno.Email)
            .IsUnique();

        modelBuilder.Entity<Aluno>()
            .HasIndex(aluno => aluno.Matricula)
            .IsUnique();

        modelBuilder.Entity<Curso>()
            .HasIndex(curso => curso.Codigo)
            .IsUnique();

        modelBuilder.Entity<Disciplina>()
            .HasIndex(disciplina => disciplina.Codigo)
            .IsUnique();

        modelBuilder.Entity<MatriculaDisciplina>()
            .HasIndex(matricula => new { matricula.AlunoId, matricula.DisciplinaId })
            .IsUnique();
        
        // Dados estaticos para teste
        modelBuilder.Entity<Curso>().HasData(
            new Curso { Id = 1, Nome = "Analise e Desenvolvimento de Sistemas", Codigo = "ADS", DuracaoSemestres = 5 },
            new Curso { Id = 2, Nome = "Sistemas de Informacao", Codigo = "SI", DuracaoSemestres = 8 },
            new Curso { Id = 3, Nome = "Ciencia da Computacao", Codigo = "CC", DuracaoSemestres = 8 },
            new Curso { Id = 4, Nome = "Engenharia de Software", Codigo = "ES", DuracaoSemestres = 8 },
            new Curso { Id = 5, Nome = "Seguranca da Informacao", Codigo = "SEG", DuracaoSemestres = 6 }
        );

        // Dados estaticos para teste
        modelBuilder.Entity<Disciplina>().HasData(
            // ===== ADS (CursoId = 1) =====
            new Disciplina { Id = 1, Nome = "Algoritmos e Logica de Programacao", Codigo = "ADS101", CargaHoraria = 80, CursoId = 1 },
            new Disciplina { Id = 2, Nome = "Estrutura de Dados", Codigo = "ADS102", CargaHoraria = 80, CursoId = 1 },

            // ===== Sistemas de Informacao (CursoId = 2) =====
            new Disciplina { Id = 3, Nome = "Analise de Sistemas", Codigo = "SI101", CargaHoraria = 80, CursoId = 2 },
            new Disciplina { Id = 4, Nome = "Gestao de Projetos", Codigo = "SI102", CargaHoraria = 80, CursoId = 2 },

            // ===== Ciencia da Computacao (CursoId = 3) =====
            new Disciplina { Id = 5, Nome = "Calculo I", Codigo = "CC101", CargaHoraria = 80, CursoId = 3 },
            new Disciplina { Id = 6, Nome = "Teoria da Computacao", Codigo = "CC102", CargaHoraria = 80, CursoId = 3 },

            // ===== Engenharia de Software (CursoId = 4) =====
            new Disciplina { Id = 7, Nome = "Arquitetura de Software", Codigo = "ES101", CargaHoraria = 80, CursoId = 4 },
            new Disciplina { Id = 8, Nome = "Testes de Software", Codigo = "ES102", CargaHoraria = 80, CursoId = 4 },

            // ===== Seguranca da Informacao (CursoId = 5) =====
            new Disciplina { Id = 9, Nome = "Criptografia", Codigo = "SEG101", CargaHoraria = 80, CursoId = 5 },
            new Disciplina { Id = 10, Nome = "Seguranca de Redes", Codigo = "SEG102", CargaHoraria = 80, CursoId = 5 }
        );

        // Dados estaticos para teste
        modelBuilder.Entity<Aluno>().HasData(
            new Aluno
            {
                Id = 1,
                Nome = "Vinicius Correia",
                Email = "viniciuscorreia@academico.local",
                Matricula = "20260002",
                DataNascimento = new DateOnly(2008, 3, 17),
                Ativo = true,
                CursoId = 1
            },
            new Aluno
            {
                Id = 2,
                Nome = "Ana Souza",
                Email = "ana.souza@academico.local",
                Matricula = "20260001",
                DataNascimento = new DateOnly(2002, 4, 12),
                Ativo = false,
                CursoId = 2
            },
            new Aluno
            {
                Id = 3,
                Nome = "Mariana Lima",
                Email = "mariana.lima@academico.local",
                Matricula = "20260003",
                DataNascimento = new DateOnly(2004, 8, 25),
                Ativo = true,
                CursoId = 3
            },
            new Aluno
            {
                Id = 4,
                Nome = "Carlos Pereira",
                Email = "carlos.pereira@academico.local",
                Matricula = "20260004",
                DataNascimento = new DateOnly(2003, 11, 9),
                Ativo = true,
                CursoId = 4
            }
        );

        modelBuilder.Entity<MatriculaDisciplina>().HasData(
            new MatriculaDisciplina { Id = 1, AlunoId = 1, DisciplinaId = 1, DataMatricula = new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), Status = "Cursando" },
            new MatriculaDisciplina { Id = 2, AlunoId = 1, DisciplinaId = 2, DataMatricula = new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), Status = "Cursando" },
            new MatriculaDisciplina { Id = 3, AlunoId = 3, DisciplinaId = 5, DataMatricula = new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), Status = "Cursando" },
            new MatriculaDisciplina { Id = 4, AlunoId = 3, DisciplinaId = 6, DataMatricula = new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), Status = "Cursando" },
            new MatriculaDisciplina { Id = 5, AlunoId = 4, DisciplinaId = 7, DataMatricula = new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), Status = "Cursando" },
            new MatriculaDisciplina { Id = 6, AlunoId = 4, DisciplinaId = 8, DataMatricula = new DateTime(2026, 5, 13, 12, 0, 0, DateTimeKind.Utc), Status = "Cursando" }
        );
    }
}
