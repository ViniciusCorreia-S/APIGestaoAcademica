using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.Models;

public class Aluno
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Matricula { get; set; } = string.Empty;

    public DateOnly DataNascimento { get; set; }

    public bool Ativo { get; set; } = true;

    public int CursoId { get; set; }
    public Curso? Curso { get; set; }

    // Relacionamento muitos-para-muitos com Disciplina via MatriculaDisciplina
    public ICollection<MatriculaDisciplina> Matriculas { get; set; } = [];
}
