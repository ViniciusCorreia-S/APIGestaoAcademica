using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.DTOs;

public class AlunoCreateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    public DateOnly DataNascimento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um curso valido.")]
    public int CursoId { get; set; }
}
