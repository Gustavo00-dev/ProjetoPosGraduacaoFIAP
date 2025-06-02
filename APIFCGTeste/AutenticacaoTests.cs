using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace APIFCGTeste;

public class CustomWebApplicationFactory : WebApplicationFactory<APIFCG.Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }
}

public class AutenticacaoTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AutenticacaoTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornarTokenJwt()
    {
        var loginRequest = new
        {
            Email = "gustavo@rodrigues.com",
            Password = "12345abc@"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);

        Assert.True(document.RootElement.TryGetProperty("token", out var token));
        Assert.False(string.IsNullOrWhiteSpace(token.GetString()));
    }

    [Fact]
    public async Task Login_ComCredenciaisInvalidas_DeveNegarAcesso()
    {
        var loginRequest = new
        {
            Email = "gustavo@rodrigues.com",
            Password = "12345abc"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Acesso_SemToken_DeveRetornar401()
    {
        var response = await _client.GetAsync("/api/Jogo/BuscarTodosJogos");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Acesso_ComTokenInvalido_DeveRetornar401()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/Jogo/BuscarTodosJogos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token_invalido_ou_expirado");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Acesso_ComTokenValido_DeveRetornar200()
    {
        var loginRequest = new
        {
            Email = "gustavo@rodrigues.com",
            Password = "12345abc@"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var json = await loginResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var token = document.RootElement.GetProperty("token").GetString();

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/Jogo/BuscarTodosJogos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
