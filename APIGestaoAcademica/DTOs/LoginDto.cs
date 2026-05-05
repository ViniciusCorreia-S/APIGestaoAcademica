using System.ComponentModel.DataAnnotations;

namespace GestaoAcademica.DTOs;

public class LoginDto
{
    [Required]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;
}
