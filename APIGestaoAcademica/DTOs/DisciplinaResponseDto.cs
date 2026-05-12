namespace GestaoAcademica.DTOs;

public class DisciplinaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public int CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
}
