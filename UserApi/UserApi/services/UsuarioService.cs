using Microsoft.AspNetCore.Identity;

namespace UserApi.services;

public class UsuarioService
{
    private readonly UserManager<IdentityUser<Guid>> userManager;

    public UsuarioService(UserManager<IdentityUser<Guid>> userManager)
    {
        this.userManager = userManager;
    }

    public IdentityUser<Guid>? BuscaUsuario(string username)
    {
        IdentityUser<Guid>? identityUser = userManager
            .Users
            .FirstOrDefault(u => u.NormalizedUserName == username.ToUpper() ||
                u.NormalizedEmail == username.ToUpper());

        return identityUser;
    }
}
