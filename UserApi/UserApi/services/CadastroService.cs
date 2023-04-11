using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.services;

public class CadastroService
{
    private readonly UserManager<IdentityUser<Guid>> userManager;
    private readonly IMapper mapper;

    public CadastroService(UserManager<IdentityUser<Guid>> userManager, IMapper mapper)
    {
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<Result> CadastraUsuario(CreateUsuarioDto createDto)
    {
        Usuario usuario = mapper.Map<Usuario>(createDto);
        IdentityUser<Guid> usuarioIdentity = mapper.Map<IdentityUser<Guid>>(usuario);
        IdentityResult resultadoIdentity = await userManager
            .CreateAsync(usuarioIdentity, createDto.Password);

        if (resultadoIdentity.Succeeded) return Result.Ok();
        return ListarErros(resultadoIdentity);
    }

    private Result ListarErros(IdentityResult resultadoIdentity)
    {
        var errors = resultadoIdentity.Errors
            .Select(error => error.Code).ToList();
        return Result.Fail(errors);
    }
}
