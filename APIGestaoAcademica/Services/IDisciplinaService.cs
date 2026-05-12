using GestaoAcademica.DTOs;

namespace GestaoAcademica.Services;

public interface IDisciplinaService
{
    Task<List<DisciplinaResponseDto>> ListarTodosAsync();
    Task<DisciplinaResponseDto?> BuscarPorIdAsync(int id);
    Task<DisciplinaResponseDto> CriarAsync(DisciplinaCreateDto dto);
    Task<bool> AtualizarAsync(int id, DisciplinaUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}
