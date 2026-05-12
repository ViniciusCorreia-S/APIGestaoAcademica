using GestaoAcademica.DTOs;
using GestaoAcademica.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAcademica.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DisciplinasController : ControllerBase
{
    private readonly IDisciplinaService _service;

    public DisciplinasController(IDisciplinaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(new
        {
            sucesso = true,
            dados = await _service.ListarTodosAsync()
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var disciplina = await _service.BuscarPorIdAsync(id);
        return disciplina is null
            ? NotFound(new { sucesso = false, mensagem = "Disciplina nao encontrada." })
            : Ok(new { sucesso = true, dados = disciplina });
    }

    [HttpPost]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Post([FromBody] DisciplinaCreateDto dto)
    {
        try
        {
            var disciplina = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = disciplina.Id }, new
            {
                sucesso = true,
                mensagem = "Disciplina cadastrada com sucesso.",
                dados = disciplina
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Put(int id, [FromBody] DisciplinaUpdateDto dto)
    {
        try
        {
            var atualizada = await _service.AtualizarAsync(id, dto);
            return atualizada
                ? Ok(new { sucesso = true, mensagem = "Disciplina atualizada com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Disciplina nao encontrada." });
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
        try
        {
            var removida = await _service.RemoverAsync(id);
            return removida
                ? Ok(new { sucesso = true, mensagem = "Disciplina removida com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Disciplina nao encontrada." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }
}
