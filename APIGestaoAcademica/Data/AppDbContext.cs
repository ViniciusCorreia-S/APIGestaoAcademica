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

        modelBuilder.Entity<Curso>().HasData(
            new Curso { Id = 1, Nome = "Analise e Desenvolvimento de Sistemas", Codigo = "ADS", DuracaoSemestres = 5 },
            new Curso { Id = 2, Nome = "Sistemas de Informacao", Codigo = "SI", DuracaoSemestres = 8 });

        modelBuilder.Entity<Disciplina>().HasData(
            new Disciplina { Id = 1, Nome = "Programacao Web", Codigo = "WEB101", CargaHoraria = 80 },
            new Disciplina { Id = 2, Nome = "Banco de Dados", Codigo = "BD101", CargaHoraria = 80 });

        modelBuilder.Entity<Aluno>().HasData(
            new Aluno
            {
                Id = 1,
                Nome = "Ana Souza",
                Email = "ana.souza@academico.local",
                Matricula = "20260001",
                DataNascimento = new DateOnly(2002, 4, 12),
                Ativo = true,
                CursoId = 1
            });
    }
}
