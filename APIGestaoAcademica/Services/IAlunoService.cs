using GestaoAcademica.DTOs;

namespace GestaoAcademica.Services;

public interface IAlunoService
{
    Task<List<AlunoResponseDto>> ListarTodosAsync();
    Task<AlunoResponseDto?> BuscarPorIdAsync(int id);
    Task<AlunoResponseDto> CriarAsync(AlunoCreateDto dto);
    Task<bool> AtualizarAsync(int id, AlunoUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}
