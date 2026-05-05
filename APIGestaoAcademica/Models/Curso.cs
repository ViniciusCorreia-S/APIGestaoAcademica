using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.Models;

public class Curso
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    public int DuracaoSemestres { get; set; }

    public ICollection<Aluno> Alunos { get; set; } = [];
}
