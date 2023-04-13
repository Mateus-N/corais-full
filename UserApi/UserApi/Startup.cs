using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.services;
using UserApi.Services;

namespace UserApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        string? connectionString = Configuration
            .GetConnectionString("UsuarioConnection");

        services.AddDbContext<UserDbContext>(options =>
            options.UseMySQL(connectionString)
        );
        services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();
        
        services.AddScoped<HobbiesConnectionService, HobbiesConnectionService>();
        services.AddScoped<CadastroService, CadastroService>();
        services.AddScoped<UsuarioService, UsuarioService>();
        services.AddScoped<LoginService, LoginService>();
        services.AddScoped<TokenService, TokenService>();
        services.AddScoped<HttpClient, HttpClient>();

        services.AddControllers();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
