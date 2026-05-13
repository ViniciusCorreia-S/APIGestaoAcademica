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

    public async Task<CursoResponseDto?> BuscarPorIdAsync(int id)
    {
        var curso = await _repository.BuscarPorIdAsync(id);
        return curso is null ? null : MapearCurso(curso);
    }

    public async Task<CursoResponseDto> CriarAsync(CursoCreateDto dto)
    {
        var codigo = NormalizarCodigo(dto.Codigo);

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

    public async Task<bool> AtualizarAsync(int id, CursoUpdateDto dto)
    {
        var curso = await _repository.BuscarPorIdAsync(id);
        if (curso is null)
            return false;

        var codigo = NormalizarCodigo(dto.Codigo);
        if (await _repository.ExisteCodigoAsync(codigo, id))
            throw new InvalidOperationException("Ja existe outro curso com este codigo.");

        curso.Nome = dto.Nome.Trim();
        curso.Codigo = codigo;
        curso.DuracaoSemestres = dto.DuracaoSemestres;

        await _repository.AtualizarAsync(curso);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var curso = await _repository.BuscarPorIdAsync(id);
        if (curso is null)
            return false;

        if (await _repository.PossuiVinculosAsync(id))
            throw new InvalidOperationException("Nao e possivel excluir curso com alunos ou disciplinas vinculados.");

        await _repository.RemoverAsync(curso);
        return true;
    }

    private static string NormalizarCodigo(string codigo)
    {
        return codigo.Trim().ToUpperInvariant();
    }

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
