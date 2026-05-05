namespace GestaoAcademica.DTOs;

public class CursoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int DuracaoSemestres { get; set; }
}
