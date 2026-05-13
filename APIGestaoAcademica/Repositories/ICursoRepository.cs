using GestaoAcademica.Models;

namespace GestaoAcademica.Repositories;

public interface ICursoRepository
{
    Task<List<Curso>> ListarTodosAsync();
    Task<Curso?> BuscarPorIdAsync(int id);
    Task<bool> ExisteCodigoAsync(string codigo, int? ignorarCursoId = null);
    Task<bool> PossuiVinculosAsync(int cursoId);
    Task AdicionarAsync(Curso curso);
    Task AtualizarAsync(Curso curso);
    Task RemoverAsync(Curso curso);
}
