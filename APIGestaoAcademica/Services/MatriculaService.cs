using GestaoAcademica.DTOs;
using GestaoAcademica.Models;
using GestaoAcademica.Repositories;

namespace GestaoAcademica.Services;

public class MatriculaService : IMatriculaService
{
    private readonly IMatriculaRepository _matriculaRepository;
    private readonly IAlunoRepository _alunoRepository;

    public MatriculaService(IMatriculaRepository matriculaRepository, IAlunoRepository alunoRepository)
    {
        _matriculaRepository = matriculaRepository;
        _alunoRepository = alunoRepository;
    }

    public async Task<List<MatriculaResponseDto>> ListarTodosAsync()
    {
        var matriculas = await _matriculaRepository.ListarTodosAsync();
        return matriculas.Select(MapearMatricula).ToList();
    }

    public async Task<MatriculaResponseDto> CriarAsync(MatriculaCreateDto dto)
    {
        // Validar se o aluno existe e está ativo
        var aluno = await _alunoRepository.BuscarPorIdAsync(dto.AlunoId);
        if (aluno is null)
            throw new InvalidOperationException("Aluno informado nao foi encontrado.");

        // Validar se o aluno está ativo
        if (!aluno.Ativo)
            throw new InvalidOperationException("Nao e possivel matricular aluno inativo.");

        // Validar se a disciplina existe
        if (!await _matriculaRepository.ExisteDisciplinaAsync(dto.DisciplinaId))
            throw new InvalidOperationException("Disciplina informada nao foi encontrada.");

        // Validar se o aluno já está matriculado na disciplina
        if (await _matriculaRepository.ExisteMatriculaAsync(dto.AlunoId, dto.DisciplinaId))
            throw new InvalidOperationException("Aluno ja esta matriculado nesta disciplina.");

        var matricula = new MatriculaDisciplina
        {
            AlunoId = dto.AlunoId,
            DisciplinaId = dto.DisciplinaId,
            DataMatricula = DateTime.UtcNow,
            Status = "Cursando"
        };

        await _matriculaRepository.AdicionarAsync(matricula);
        var matriculaCriada = await _matriculaRepository.BuscarPorIdAsync(matricula.Id);

        return MapearMatricula(matriculaCriada!);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var matricula = await _matriculaRepository.BuscarPorIdAsync(id);
        if (matricula is null)
            return false;

        await _matriculaRepository.RemoverAsync(matricula);
        return true;
    }

    // Método auxiliar para mapear MatriculaDisciplina para MatriculaResponseDto
    private static MatriculaResponseDto MapearMatricula(MatriculaDisciplina matricula)
    {
        return new MatriculaResponseDto
        {
            Id = matricula.Id,
            AlunoId = matricula.AlunoId,
            AlunoNome = matricula.Aluno?.Nome ?? string.Empty,
            DisciplinaId = matricula.DisciplinaId,
            DisciplinaNome = matricula.Disciplina?.Nome ?? string.Empty,
            DataMatricula = matricula.DataMatricula,
            Status = matricula.Status
        };
    }
}
