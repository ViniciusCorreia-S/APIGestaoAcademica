using GestaoAcademica.DTOs;
using GestaoAcademica.Models;
using GestaoAcademica.Repositories;

namespace GestaoAcademica.Services;

public class CursoService : ICursoService
{
    private readonly ICursoRepository _repository;

    public CursoService(ICursoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CursoResponseDto>> ListarTodosAsync()
    {
        var cursos = await _repository.ListarTodosAsync();
        return cursos.Select(MapearCurso).ToList();
    }

    public async Task<CursoResponseDto> CriarAsync(CursoCreateDto dto)
    {
        var codigo = dto.Codigo.Trim()
            // Remove espaços e converte para maiúsculas
            .ToUpperInvariant();

        if (await _repository.ExisteCodigoAsync(codigo))
            throw new InvalidOperationException("Ja existe curso com este codigo.");

        var curso = new Curso
        {
            Nome = dto.Nome.Trim(),
            Codigo = codigo,
            DuracaoSemestres = dto.DuracaoSemestres
        };

        await _repository.AdicionarAsync(curso);
        return MapearCurso(curso);
    }

    // Método privado para mapear Curso para CursoResponseDto
    private static CursoResponseDto MapearCurso(Curso curso)
    {
        return new CursoResponseDto
        {
            Id = curso.Id,
            Nome = curso.Nome,
            Codigo = curso.Codigo,
            DuracaoSemestres = curso.DuracaoSemestres
        };
    }
}
