namespace UserApi.Models;

public class TokenJsonToReturn
{
    public string Token { get; set; }

    public TokenJsonToReturn(string token)
    {
        Token = token;
    }
}
