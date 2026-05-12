using GestaoAcademica.DTOs;
using GestaoAcademica.Models;
using GestaoAcademica.Repositories;

namespace GestaoAcademica.Services;

public class DisciplinaService : IDisciplinaService
{
    private readonly IDisciplinaRepository _disciplinaRepository;
    private readonly ICursoRepository _cursoRepository;

    public DisciplinaService(IDisciplinaRepository disciplinaRepository, ICursoRepository cursoRepository)
    {
        _disciplinaRepository = disciplinaRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<List<DisciplinaResponseDto>> ListarTodosAsync()
    {
        var disciplinas = await _disciplinaRepository.ListarTodosAsync();
        return disciplinas.Select(MapearDisciplina).ToList();
    }

    public async Task<DisciplinaResponseDto?> BuscarPorIdAsync(int id)
    {
        var disciplina = await _disciplinaRepository.BuscarPorIdAsync(id);
        return disciplina is null ? null : MapearDisciplina(disciplina);
    }

    public async Task<DisciplinaResponseDto> CriarAsync(DisciplinaCreateDto dto)
    {
        await ValidarCursoAsync(dto.CursoId);

        var codigo = NormalizarCodigo(dto.Codigo);
        if (await _disciplinaRepository.ExisteCodigoAsync(codigo))
            throw new InvalidOperationException("Ja existe disciplina com este codigo.");

        var disciplina = new Disciplina
        {
            Nome = dto.Nome.Trim(),
            Codigo = codigo,
            CargaHoraria = dto.CargaHoraria,
            CursoId = dto.CursoId
        };

        await _disciplinaRepository.AdicionarAsync(disciplina);
        var disciplinaCriada = await _disciplinaRepository.BuscarPorIdAsync(disciplina.Id);

        return MapearDisciplina(disciplinaCriada!);
    }

    public async Task<bool> AtualizarAsync(int id, DisciplinaUpdateDto dto)
    {
        var disciplina = await _disciplinaRepository.BuscarPorIdAsync(id);
        if (disciplina is null)
            return false;

        await ValidarCursoAsync(dto.CursoId);

        var codigo = NormalizarCodigo(dto.Codigo);
        if (await _disciplinaRepository.ExisteCodigoAsync(codigo, id))
            throw new InvalidOperationException("Ja existe outra disciplina com este codigo.");

        disciplina.Nome = dto.Nome.Trim();
        disciplina.Codigo = codigo;
        disciplina.CargaHoraria = dto.CargaHoraria;
        disciplina.CursoId = dto.CursoId;

        await _disciplinaRepository.AtualizarAsync(disciplina);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var disciplina = await _disciplinaRepository.BuscarPorIdAsync(id);
        if (disciplina is null)
            return false;

        if (await _disciplinaRepository.PossuiMatriculasAsync(id))
            throw new InvalidOperationException("Nao e possivel excluir disciplina com matriculas vinculadas.");

        await _disciplinaRepository.RemoverAsync(disciplina);
        return true;
    }

    private async Task ValidarCursoAsync(int cursoId)
    {
        var curso = await _cursoRepository.BuscarPorIdAsync(cursoId);
        if (curso is null)
            throw new InvalidOperationException("Curso informado nao foi encontrado.");
    }

    private static string NormalizarCodigo(string codigo)
    {
        return codigo.Trim().ToUpperInvariant();
    }

    private static DisciplinaResponseDto MapearDisciplina(Disciplina disciplina)
    {
        return new DisciplinaResponseDto
        {
            Id = disciplina.Id,
            Nome = disciplina.Nome,
            Codigo = disciplina.Codigo,
            CargaHoraria = disciplina.CargaHoraria,
            CursoId = disciplina.CursoId,
            CursoNome = disciplina.Curso?.Nome ?? string.Empty
        };
    }
}
