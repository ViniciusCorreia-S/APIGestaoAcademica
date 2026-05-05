using GestaoAcademica.Models;

namespace GestaoAcademica.Repositories;

public interface IMatriculaRepository
{
    Task<List<MatriculaDisciplina>> ListarTodosAsync();
    Task<MatriculaDisciplina?> BuscarPorIdAsync(int id);
    Task<bool> ExisteMatriculaAsync(int alunoId, int disciplinaId);
    Task<bool> ExisteDisciplinaAsync(int disciplinaId);
    Task AdicionarAsync(MatriculaDisciplina matricula);
    Task RemoverAsync(MatriculaDisciplina matricula);
}
