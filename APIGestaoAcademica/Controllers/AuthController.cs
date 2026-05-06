using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoAcademica.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GestaoAcademica.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        var perfil = ValidarUsuario(login.Usuario, login.Senha);
        // Se o perfil for null, significa que as credenciais são inválidas
        if (perfil is null)
        {
            return Unauthorized(new
            {
                sucesso = false,
                mensagem = "Usuario ou senha invalidos."
            });
        }

        var token = GerarToken(login.Usuario, perfil);

        return Ok(new
        {
            sucesso = true,
            usuario = login.Usuario,
            perfil,
            token
        });
    }

    private static string? ValidarUsuario(string usuario, string senha)
    {
        return (usuario, senha) switch
        {
            ("admin", "123456") => "Administrador",
            ("secretaria", "123456") => "Secretaria",
            _ => null
        };
    }

    private string GerarToken(string usuario, string perfil)
    {
        // Obter a chave secreta do appsettings.json
        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key nao configurada.");

        // Converter a chave secreta para bytes
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, perfil)
            }),
            // O token expira em 2 horas
            Expires = DateTime.UtcNow.AddHours(2),
            // Assinar o token usando a chave secreta e o algoritmo HMAC SHA256
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            // Definir o emissor e o público do token
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        // Criar o token JWT usando o token descriptor
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
