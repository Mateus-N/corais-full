using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using UserApi.Dtos;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.services;

public class CadastroService
{
    private readonly HobbiesConnectionService hobbiesConnectionService;
    private readonly UserManager<IdentityUser<Guid>> userManager;
    private readonly UsuarioService usuarioService;
    private readonly IMapper mapper;

    public CadastroService(
        HobbiesConnectionService hobbiesConnectionService,
        UserManager<IdentityUser<Guid>> userManager,
        UsuarioService usuarioService,
        IMapper mapper
        )
    {
        this.hobbiesConnectionService = hobbiesConnectionService;
        this.usuarioService = usuarioService;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<Result> CadastraUsuario(CreateUsuarioDto createDto)
    {
        Usuario usuario = mapper.Map<Usuario>(createDto);
        IdentityUser<Guid> usuarioIdentity = mapper.Map<IdentityUser<Guid>>(usuario);
        IdentityResult resultadoIdentity = await userManager
            .CreateAsync(usuarioIdentity, createDto.Password);

        if (resultadoIdentity.Succeeded)
        {
            IdentityUser<Guid>? fullUsuario = usuarioService
                .BuscaUsuario(usuarioIdentity.UserName);
            await hobbiesConnectionService.SendToEmailAsync(fullUsuario);
            return Result.Ok();
        }

        return ListarErros(resultadoIdentity);
    }

    private Result ListarErros(IdentityResult resultadoIdentity)
    {
        List<string> errors = resultadoIdentity.Errors
            .Select(error => error.Code).ToList();
        return Result.Fail(errors);
    }
}
