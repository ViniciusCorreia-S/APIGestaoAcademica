using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.DTOs;

public class CursoCreateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Range(1, 12)]
    public int DuracaoSemestres { get; set; }
}
