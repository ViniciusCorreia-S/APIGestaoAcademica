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

    public async Task<bool> ExisteCodigoAsync(string codigo)
    {
        return await _context.Cursos.AnyAsync(curso => curso.Codigo == codigo);
    }

    public async Task AdicionarAsync(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
        await _context.SaveChangesAsync();
    }
}
