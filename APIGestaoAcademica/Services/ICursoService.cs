using GestaoAcademica.DTOs;

namespace GestaoAcademica.Services;

public interface ICursoService
{
    Task<List<CursoResponseDto>> ListarTodosAsync();
    Task<CursoResponseDto?> BuscarPorIdAsync(int id);
    Task<CursoResponseDto> CriarAsync(CursoCreateDto dto);
    Task<bool> AtualizarAsync(int id, CursoUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}
