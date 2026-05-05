using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.DTOs;

public class MatriculaCreateDto
{
    [Range(1, int.MaxValue)]
    public int AlunoId { get; set; }

    [Range(1, int.MaxValue)]
    public int DisciplinaId { get; set; }
}
