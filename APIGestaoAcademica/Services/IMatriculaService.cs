using GestaoAcademica.DTOs;

namespace GestaoAcademica.Services;

public interface IMatriculaService
{
    Task<List<MatriculaResponseDto>> ListarTodosAsync();
    Task<MatriculaResponseDto> CriarAsync(MatriculaCreateDto dto);
    Task<bool> RemoverAsync(int id);
}
