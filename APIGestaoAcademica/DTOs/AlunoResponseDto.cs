namespace GestaoAcademica.DTOs;

public class AlunoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public bool Ativo { get; set; }
    public int CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
}
