using GestaoAcademica.DTOs;
using GestaoAcademica.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAcademica.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CursosController : ControllerBase
{
    private readonly ICursoService _service;

    public CursosController(ICursoService service)
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
        var curso = await _service.BuscarPorIdAsync(id);
        return curso is null
            ? NotFound(new { sucesso = false, mensagem = "Curso nao encontrado." })
            : Ok(new { sucesso = true, dados = curso });
    }

    [HttpPost]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Post([FromBody] CursoCreateDto dto)
    {
        try
        {
            var curso = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = curso.Id }, new
            {
                sucesso = true,
                mensagem = "Curso cadastrado com sucesso.",
                dados = curso
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Put(int id, [FromBody] CursoUpdateDto dto)
    {
        try
        {
            var atualizado = await _service.AtualizarAsync(id, dto);
            return atualizado
                ? Ok(new { sucesso = true, mensagem = "Curso atualizado com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Curso nao encontrado." });
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
            var removido = await _service.RemoverAsync(id);
            return removido
                ? Ok(new { sucesso = true, mensagem = "Curso removido com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Curso nao encontrado." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }
}
