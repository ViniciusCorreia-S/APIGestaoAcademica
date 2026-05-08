using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.Models;

public class Disciplina
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    public int CargaHoraria { get; set; }

    // Relacionamento muitos-para-muitos com Aluno via MatriculaDisciplina
    public ICollection<MatriculaDisciplina> Matriculas { get; set; } = [];
}
