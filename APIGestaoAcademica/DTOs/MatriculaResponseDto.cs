namespace GestaoAcademica.DTOs;

public class MatriculaResponseDto
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public int DisciplinaId { get; set; }
    public string DisciplinaNome { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public string Status { get; set; } = string.Empty;
}
