using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;

namespace UserApi.Services;

public class HobbiesConnectionService
{
    private readonly HttpClient httpClient;

    public HobbiesConnectionService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task SendToEmailAsync(IdentityUser<Guid> usuario)
    {
        string json = JsonConvert.SerializeObject(new
        {
            usuario.Id,
            Name = usuario.UserName,
            usuario.Email
        });

        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = await httpClient
            .PostAsync("http://172.17.0.1:8081/api/usuario", httpContent);
        httpResponse.EnsureSuccessStatusCode();
    }
}
