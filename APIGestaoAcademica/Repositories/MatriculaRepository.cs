using GestaoAcademica.Data;
using GestaoAcademica.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademica.Repositories;

public class MatriculaRepository : IMatriculaRepository
{
    private readonly AppDbContext _context;

    public MatriculaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MatriculaDisciplina>> ListarTodosAsync()
    {
        return await _context.MatriculasDisciplinas
            .Include(matricula => matricula.Aluno)
            .Include(matricula => matricula.Disciplina)
            .OrderByDescending(matricula => matricula.DataMatricula)
            .ToListAsync();
    }

    public async Task<MatriculaDisciplina?> BuscarPorIdAsync(int id)
    {
        return await _context.MatriculasDisciplinas
            .Include(matricula => matricula.Aluno)
            .Include(matricula => matricula.Disciplina)
            .FirstOrDefaultAsync(matricula => matricula.Id == id);
    }

    public async Task<bool> ExisteMatriculaAsync(int alunoId, int disciplinaId)
    {
        return await _context.MatriculasDisciplinas.AnyAsync(matricula =>
            matricula.AlunoId == alunoId && matricula.DisciplinaId == disciplinaId);
    }

    public async Task<bool> ExisteDisciplinaAsync(int disciplinaId)
    {
        return await _context.Disciplinas.AnyAsync(disciplina => disciplina.Id == disciplinaId);
    }

    public async Task AdicionarAsync(MatriculaDisciplina matricula)
    {
        await _context.MatriculasDisciplinas.AddAsync(matricula);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(MatriculaDisciplina matricula)
    {
        _context.MatriculasDisciplinas.Remove(matricula);
        await _context.SaveChangesAsync();
    }
}
