using GestaoAcademica.Models;

namespace GestaoAcademica.Repositories;

public interface ICursoRepository
{
    Task<List<Curso>> ListarTodosAsync();
    Task<Curso?> BuscarPorIdAsync(int id);
    Task<bool> ExisteCodigoAsync(string codigo);
    Task AdicionarAsync(Curso curso);
}
