using GestaoAcademica.DTOs;

namespace GestaoAcademica.Services;

public interface ICursoService
{
    Task<List<CursoResponseDto>> ListarTodosAsync();
    Task<CursoResponseDto> CriarAsync(CursoCreateDto dto);
}
