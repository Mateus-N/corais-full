using AplicationApi.Dtos;
using AplicationApi.Models;
using AplicationApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AplicationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        this.usuarioService = usuarioService;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUsuarioDto userDto)
    {
        Usuario usuario = usuarioService.CreateUser(userDto);
        return Ok(usuario);
    }

    [HttpGet]
    [Authorize]
    public IActionResult LoginComToken()
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
        Usuario? usuario = usuarioService.UsuarioPorId(userId);
        if (usuario != null) return Ok(usuario);
        return NotFound();
    }
}
