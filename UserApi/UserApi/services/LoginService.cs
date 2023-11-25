using FluentResults;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.services;

public class LoginService
{
    private readonly SignInManager<IdentityUser<Guid>> signInManager;
    private readonly UsuarioService usuarioService;
    private readonly TokenService tokenService;
    private readonly IConnectionMultiplexer redis;

    public LoginService(
        SignInManager<IdentityUser<Guid>> signInManager,
        TokenService tokenService,
        UsuarioService usuarioService,
        IConnectionMultiplexer redis
        )
    {
        this.signInManager = signInManager;
        this.tokenService = tokenService;
        this.usuarioService = usuarioService;
        this.redis = redis;
    }

    public async Task<Result> LogaUsuario(LoginRequest request)
    {
        string key = CriarKey(request);
        string? token = await VerificaExistenciaDoToken(key);
        if (token != null) return Result.Ok().WithSuccess(token);

        IdentityUser<Guid>? user = usuarioService.BuscaUsuario(request.Username);
        if (user != null)
        {
            SignInResult identityResult = await signInManager
                .PasswordSignInAsync(user.UserName!, request.Password, false, false);
            if (identityResult.Succeeded)
            {
                string newToken = tokenService.CreateToken(user).Value;
                IDatabase db = redis.GetDatabase();
                await db.StringSetAsync(key, newToken, TimeSpan.FromHours(1));
                return Result.Ok().WithSuccess(newToken);
            }
        }
        return Result.Fail("Login ou senha inválidos!");
    }

    private async Task<string?> VerificaExistenciaDoToken(string key)
    {
        IDatabase db = redis.GetDatabase();
        RedisValue token = await db.StringGetAsync(key);

        if (token.HasValue) return token;
        return null;
    }

    private static string CriarKey(LoginRequest request)
    {
        return $"{request.Username}{request.Password}";
    }

    public TokenJsonToReturn CriaTokenJson(ISuccess success)
    {
        TokenJsonToReturn token = new(success.Message);
        return token;
    }
}
