using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UserApi.Dtos;
using UserApi.services;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginService service;

    public LoginController(LoginService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> LogaUsuario(LoginRequest request)
    {
        Result resultado = await service.LogaUsuario(request);
        if (resultado.IsSuccess) return Ok(resultado.Successes.First());
        return Unauthorized();
    }
}
