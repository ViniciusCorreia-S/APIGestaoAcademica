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

    [HttpPost]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Post([FromBody] CursoCreateDto dto)
    {
        try
        {
            var curso = await _service.CriarAsync(dto);
            return Created(string.Empty, new
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
}
