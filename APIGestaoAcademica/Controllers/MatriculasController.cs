using GestaoAcademica.DTOs;
using GestaoAcademica.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAcademica.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "EquipeAcademica")]
public class MatriculasController : ControllerBase
{
    private readonly IMatriculaService _service;

    public MatriculasController(IMatriculaService service)
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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MatriculaCreateDto dto)
    {
        try
        {
            var matricula = await _service.CriarAsync(dto);
            return Created(string.Empty, new
            {
                sucesso = true,
                mensagem = "Matricula em disciplina realizada com sucesso.",
                dados = matricula
            });
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
        var removida = await _service.RemoverAsync(id);
        return removida
            ? Ok(new { sucesso = true, mensagem = "Matricula removida com sucesso." })
            : NotFound(new { sucesso = false, mensagem = "Matricula nao encontrada." });
    }
}
