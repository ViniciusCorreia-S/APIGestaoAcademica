using GestaoAcademica.Data;
using GestaoAcademica.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademica.Repositories;

public class DisciplinaRepository : IDisciplinaRepository
{
    private readonly AppDbContext _context;

    public DisciplinaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Disciplina>> ListarTodosAsync()
    {
        return await _context.Disciplinas
            .Include(disciplina => disciplina.Curso)
            .OrderBy(disciplina => disciplina.Nome)
            .ToListAsync();
    }

    public async Task<Disciplina?> BuscarPorIdAsync(int id)
    {
        return await _context.Disciplinas
            .Include(disciplina => disciplina.Curso)
            .FirstOrDefaultAsync(disciplina => disciplina.Id == id);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? ignorarDisciplinaId = null)
    {
        return await _context.Disciplinas.AnyAsync(disciplina =>
            disciplina.Codigo == codigo && (!ignorarDisciplinaId.HasValue || disciplina.Id != ignorarDisciplinaId));
    }

    public async Task<bool> PossuiMatriculasAsync(int disciplinaId)
    {
        return await _context.MatriculasDisciplinas.AnyAsync(matricula => matricula.DisciplinaId == disciplinaId);
    }

    public async Task AdicionarAsync(Disciplina disciplina)
    {
        await _context.Disciplinas.AddAsync(disciplina);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Disciplina disciplina)
    {
        _context.Disciplinas.Update(disciplina);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Disciplina disciplina)
    {
        _context.Disciplinas.Remove(disciplina);
        await _context.SaveChangesAsync();
    }
}
