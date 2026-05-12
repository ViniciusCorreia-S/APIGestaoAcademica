using GestaoAcademica.Models;

namespace GestaoAcademica.Repositories;

public interface IDisciplinaRepository
{
    Task<List<Disciplina>> ListarTodosAsync();
    Task<Disciplina?> BuscarPorIdAsync(int id);
    Task<bool> ExisteCodigoAsync(string codigo, int? ignorarDisciplinaId = null);
    Task<bool> PossuiMatriculasAsync(int disciplinaId);
    Task AdicionarAsync(Disciplina disciplina);
    Task AtualizarAsync(Disciplina disciplina);
    Task RemoverAsync(Disciplina disciplina);
}
