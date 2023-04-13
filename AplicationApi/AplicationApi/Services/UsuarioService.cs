using AplicationApi.Data;
using AplicationApi.Dtos;
using AplicationApi.Models;
using AutoMapper;

namespace AplicationApi.Services;

public class UsuarioService
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public UsuarioService(AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public Usuario CreateUser(CreateUsuarioDto userDto)
    {
        Usuario usuario = mapper.Map<Usuario>(userDto);
        context.Users.Add(usuario);
        context.SaveChanges();
        return usuario;
    }

    public Usuario? UsuarioPorId(Guid userId)
    {
        Usuario? usuario = context.Users.FirstOrDefault(u => u.Id == userId);
        return usuario;
    }
}
