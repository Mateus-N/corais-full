using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UserApi.Data;
using UserApi.Models;
using UserApi.services;
using UserApi.Services;

namespace UserApi;

public class Startup
{
    public ConfigurationManager Configuration { get; }
    public IWebHostEnvironment Env { get; }

    public Startup(ConfigurationManager configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        GetAppSettings(services);

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

    private void GetAppSettings(IServiceCollection services)
    {
        Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Env.EnvironmentName}.json");

        HobbiesConnection? hobbies = Configuration
            .GetSection("HobbiesConnectionLink")
            .Get<HobbiesConnection>();
        services.AddSingleton(hobbies);

        RedisConnection? redis = Configuration
            .GetSection("RedisConnection")
            .Get<RedisConnection>();
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redis.StringConnection));
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Env.IsDevelopment())
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
