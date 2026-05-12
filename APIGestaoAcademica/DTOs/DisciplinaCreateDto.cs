using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.DTOs;

public class DisciplinaCreateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Range(1, 400, ErrorMessage = "Informe uma carga horaria valida.")]
    public int CargaHoraria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um curso valido.")]
    public int CursoId { get; set; }
}
