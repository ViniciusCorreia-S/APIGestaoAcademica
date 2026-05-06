using GestaoAcademica.DTOs;
using GestaoAcademica.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAcademica.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "EquipeAcademica")]
public class AlunosController : ControllerBase
{
    private readonly IAlunoService _service;

    public AlunosController(IAlunoService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        return Ok(new
        {
            sucesso = true,
            dados = await _service.ListarTodosAsync()
        });
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        // Busca o aluno pelo ID usando o serviço
        var aluno = await _service.BuscarPorIdAsync(id);
        return aluno is null
            ? NotFound(new { sucesso = false, mensagem = "Aluno nao encontrado." })
            : Ok(new { sucesso = true, dados = aluno });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AlunoCreateDto dto)
    {
        try
        {
            var aluno = await _service.CriarAsync(dto);
            // Retorna 201 Created com a localização do novo recurso e os dados do aluno criado
            return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, new
            {
                sucesso = true,
                mensagem = "Aluno cadastrado com sucesso.",
                dados = aluno
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] AlunoUpdateDto dto)
    {
        try
        {
            var atualizado = await _service.AtualizarAsync(id, dto);
            
            return atualizado
                ? Ok(new { sucesso = true, mensagem = "Aluno atualizado com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Aluno nao encontrado." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _service.RemoverAsync(id);
        return removido
            ? Ok(new { sucesso = true, mensagem = "Aluno removido com sucesso." })
            : NotFound(new { sucesso = false, mensagem = "Aluno nao encontrado." });
    }
}
