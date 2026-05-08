using GestaoAcademica.Data;
using GestaoAcademica.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademica.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly AppDbContext _context;

    public AlunoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Aluno>> ListarTodosAsync()
    {
        return await _context.Alunos
            // Inclui o curso relacionado ao aluno
            .Include(aluno => aluno.Curso)
            // Ordena os alunos por nome
            .OrderBy(aluno => aluno.Nome)
            // Converte o resultado para uma lista
            .ToListAsync();
    }

    public async Task<Aluno?> BuscarPorIdAsync(int id)
    {
        return await _context.Alunos
            // Inclui o curso relacionado ao aluno
            .Include(aluno => aluno.Curso)
            // Filtra o aluno pelo ID
            .FirstOrDefaultAsync(aluno => aluno.Id == id);
    }

    public async Task<bool> ExisteEmailAsync(string email, int? ignorarAlunoId = null)
    {
        // Verifica se existe algum aluno com o email fornecido, ignorando um aluno específico (para atualizações)
        return await _context.Alunos.AnyAsync(aluno =>
            aluno.Email == email && (!ignorarAlunoId.HasValue || aluno.Id != ignorarAlunoId));
    }

    public async Task<bool> ExisteMatriculaAsync(string matricula)
    {
        return await _context.Alunos.AnyAsync(aluno => aluno.Matricula == matricula);
    }

    public async Task AdicionarAsync(Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Aluno aluno)
    {
        _context.Alunos.Update(aluno);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        await _context.SaveChangesAsync();
    }
}
