using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UserApi.Dtos;
using UserApi.services;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CadastroController : ControllerBase
{
    private readonly CadastroService service;

    public CadastroController(CadastroService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CadastraUsuario(CreateUsuarioDto createDto)
    {
        Result resultado = await service.CadastraUsuario(createDto);
        if (resultado.IsSuccess) return Ok();

        var errors = resultado.Errors.Select(error => error.Message).ToList();
        return Conflict(new { errors });
    }
}
