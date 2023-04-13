using System.ComponentModel.DataAnnotations;

namespace AplicationApi.Dtos;

public class CreateUsuarioDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
}
