using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
using UserApi.Models;

namespace UserApi.Services;

public class HobbiesConnectionService
{
    private readonly HttpClient httpClient;
    private readonly HobbiesConnection hobbies;

    public HobbiesConnectionService(HttpClient httpClient, HobbiesConnection hobbies)
    {
        this.httpClient = httpClient;
        this.hobbies = hobbies;
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
            .PostAsync(hobbies.Url, httpContent);
        httpResponse.EnsureSuccessStatusCode();
    }
}
