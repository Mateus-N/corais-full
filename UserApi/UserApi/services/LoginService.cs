using FluentResults;
using Microsoft.AspNetCore.Identity;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.services;

public class LoginService
{
    private SignInManager<IdentityUser<Guid>> signInManager { get; set; }
    private readonly TokenService tokenService;

    public LoginService(SignInManager<IdentityUser<Guid>> signInManager, TokenService tokenService)
    {
        this.signInManager = signInManager;
        this.tokenService = tokenService;
    }

    public async Task<Result> LogaUsuario(LoginRequest request)
    {
        IdentityUser<Guid>? user = BuscaUsuario(request.Username);

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

    private IdentityUser<Guid>? BuscaUsuario(string username)
    {
        IdentityUser<Guid>? identityUser = signInManager
            .UserManager
            .Users
            .FirstOrDefault(u => u.NormalizedUserName == username.ToUpper() ||
                u.NormalizedEmail == username.ToUpper());

        return identityUser;
    }
}
