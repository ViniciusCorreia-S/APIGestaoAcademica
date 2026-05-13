using GestaoAcademica.Data;
using GestaoAcademica.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademica.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly AppDbContext _context;

    public CursoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Curso>> ListarTodosAsync()
    {
        return await _context.Cursos.OrderBy(curso => curso.Nome).ToListAsync();
    }

    public async Task<Curso?> BuscarPorIdAsync(int id)
    {
        return await _context.Cursos.FindAsync(id);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? ignorarCursoId = null)
    {
        return await _context.Cursos.AnyAsync(curso =>
            curso.Codigo == codigo && (!ignorarCursoId.HasValue || curso.Id != ignorarCursoId));
    }

    public async Task<bool> PossuiVinculosAsync(int cursoId)
    {
        return await _context.Alunos.AnyAsync(aluno => aluno.CursoId == cursoId)
            || await _context.Disciplinas.AnyAsync(disciplina => disciplina.CursoId == cursoId);
    }

    public async Task AdicionarAsync(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Curso curso)
    {
        _context.Cursos.Update(curso);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Curso curso)
    {
        _context.Cursos.Remove(curso);
        await _context.SaveChangesAsync();
    }
}
