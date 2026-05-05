using GestaoAcademica.Models;

namespace GestaoAcademica.Repositories;

public interface IAlunoRepository
{
    Task<List<Aluno>> ListarTodosAsync();
    Task<Aluno?> BuscarPorIdAsync(int id);
    Task<bool> ExisteEmailAsync(string email, int? ignorarAlunoId = null);
    Task<bool> ExisteMatriculaAsync(string matricula);
    Task AdicionarAsync(Aluno aluno);
    Task AtualizarAsync(Aluno aluno);
    Task RemoverAsync(Aluno aluno);
}
