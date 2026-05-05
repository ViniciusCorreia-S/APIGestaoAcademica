using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.Models;

public class MatriculaDisciplina
{
    public int Id { get; set; }

    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    public int DisciplinaId { get; set; }
    public Disciplina? Disciplina { get; set; }

    public DateTime DataMatricula { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Cursando";
}
