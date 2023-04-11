using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();
        CreateMap<Usuario, IdentityUser<Guid>>();
    }
}
