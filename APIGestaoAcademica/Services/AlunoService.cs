using GestaoAcademica.DTOs;
using GestaoAcademica.Models;
using GestaoAcademica.Repositories;

namespace GestaoAcademica.Services;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly ICursoRepository _cursoRepository;

    public AlunoService(IAlunoRepository alunoRepository, ICursoRepository cursoRepository)
    {
        _alunoRepository = alunoRepository;
        _cursoRepository = cursoRepository;
    }

    public async Task<List<AlunoResponseDto>> ListarTodosAsync()
    {
        var alunos = await _alunoRepository.ListarTodosAsync();
        return alunos.Select(MapearAluno).ToList();
    }

    public async Task<AlunoResponseDto?> BuscarPorIdAsync(int id)
    {
        var aluno = await _alunoRepository.BuscarPorIdAsync(id);
        return aluno is null ? null : MapearAluno(aluno);
    }

    public async Task<AlunoResponseDto> CriarAsync(AlunoCreateDto dto)
    {
        await ValidarCursoAsync(dto.CursoId);

        // Verifica se a data de nascimento é futura
        if (dto.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("A data de nascimento nao pode ser futura.");

        // Verifica se o email já existe para outro aluno
        if (await _alunoRepository.ExisteEmailAsync(dto.Email))
            throw new InvalidOperationException("Ja existe aluno cadastrado com este e-mail.");

        // Verifica se a matrícula já existe para outro aluno
        if (await _alunoRepository.ExisteMatriculaAsync(dto.Matricula))
            throw new InvalidOperationException("Ja existe aluno cadastrado com esta matricula.");

        var aluno = new Aluno
        {
            Nome = dto.Nome
                // O nome é tratado para remover espaços extras
                .Trim(),

            Email = dto.Email.Trim()
                // O email é convertido para minúsculas
                .ToLowerInvariant(),
            Matricula = dto.Matricula.Trim(),
            DataNascimento = dto.DataNascimento,
            CursoId = dto.CursoId,
            Ativo = true
        };

        await _alunoRepository.AdicionarAsync(aluno);
        var alunoCriado = await _alunoRepository.BuscarPorIdAsync(aluno.Id);

        return MapearAluno(alunoCriado!);
    }

    public async Task<bool> AtualizarAsync(int id, AlunoUpdateDto dto)
    {
        var aluno = await _alunoRepository.BuscarPorIdAsync(id);
        if (aluno is null)
            return false;

        await ValidarCursoAsync(dto.CursoId);

        if (dto.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("A data de nascimento nao pode ser futura.");

        if (await _alunoRepository.ExisteEmailAsync(dto.Email, id))
            throw new InvalidOperationException("Ja existe outro aluno usando este e-mail.");

        // Atualiza os campos do aluno,
        // garantindo que o nome e email sejam tratados de forma consistente e que a matrícula não seja alterada
        aluno.Nome = dto.Nome.Trim();

        aluno.Email = dto.Email.Trim().ToLowerInvariant();
        aluno.DataNascimento = dto.DataNascimento;
        aluno.Ativo = dto.Ativo;
        aluno.CursoId = dto.CursoId;

        await _alunoRepository.AtualizarAsync(aluno);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var aluno = await _alunoRepository.BuscarPorIdAsync(id);
        if (aluno is null)
            return false;

        await _alunoRepository.RemoverAsync(aluno);
        return true;
    }

    // Valida se o curso existe antes de criar ou atualizar um aluno
    private async Task ValidarCursoAsync(int cursoId)
    {
        var curso = await _cursoRepository.BuscarPorIdAsync(cursoId);
        if (curso is null)
            throw new InvalidOperationException("Curso informado nao foi encontrado.");
    }

    // Mapeia a entidade Aluno para o DTO de resposta, incluindo o nome do curso
    private static AlunoResponseDto MapearAluno(Aluno aluno)
    {
        return new AlunoResponseDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            Matricula = aluno.Matricula,
            DataNascimento = aluno.DataNascimento,
            Ativo = aluno.Ativo,
            CursoId = aluno.CursoId,
            CursoNome = aluno.Curso?.Nome ?? string.Empty
        };
    }
}
