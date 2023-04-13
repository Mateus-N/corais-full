using AplicationApi.Dtos;
using AplicationApi.Models;
using AutoMapper;

namespace AplicationApi.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();
    }
}
