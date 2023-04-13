using FluentResults;
using Microsoft.AspNetCore.Identity;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.services;

public class LoginService
{
    private readonly SignInManager<IdentityUser<Guid>> signInManager;
    private readonly UsuarioService usuarioService;
    private readonly TokenService tokenService;

    public LoginService(
        SignInManager<IdentityUser<Guid>> signInManager,
        TokenService tokenService,
        UsuarioService usuarioService
        )
    {
        this.signInManager = signInManager;
        this.tokenService = tokenService;
        this.usuarioService = usuarioService;
    }

    public async Task<Result> LogaUsuario(LoginRequest request)
    {
        IdentityUser<Guid>? user = usuarioService.BuscaUsuario(request.Username);

        if (user != null)
        {
            SignInResult identityResult = await signInManager
                .PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (identityResult.Succeeded)
            {
                Token token = tokenService.CreateToken(user);
                return Result.Ok().WithSuccess(token.Value);
            }
        }

        return Result.Fail("Login ou senha inválidos!");
    }
}
