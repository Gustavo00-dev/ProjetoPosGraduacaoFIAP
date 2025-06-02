using APIFCG;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

public class JogoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public JogoControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ComprarJogo_ComDadosValidos_DeveRetornarStatus200()
    {
        // 1. Realiza login e obtém o token JWT
        var loginRequest = new
        {
            Email = "gustavo@rodrigues.com",
            Password = "12345abc@"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(loginContent);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();

        // 2. Adiciona o token no header Authorization
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Arrange: IDs válidos (assumindo que o jogo e o usuário existem no banco)
        int idUsuario = 1;
        int idJogo = 2;
        var endpoint = $"/api/Jogo/ComprarJogo?idUsuario={idUsuario}&idJogo={idJogo}";

        // Act
        var response = await _client.PostAsync(endpoint, null); // Corpo vazio pois params estão na query string

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var conteudo = await response.Content.ReadAsStringAsync();
        Assert.Contains("comprado com sucesso", conteudo.ToLower());
    }

    [Fact]
    public async Task ComprarJogo_ComIdUsuarioInvalido_DeveRetornarBadRequest()
    {
        // 1. Realiza login e obtém o token JWT
        var loginRequest = new
        {
            Email = "gustavo@rodrigues.com",
            Password = "12345abc@"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(loginContent);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();

        // 2. Adiciona o token no header Authorization
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync("/api/Jogo/ComprarJogo?idUsuario=0&idJogo=2", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var conteudo = await response.Content.ReadAsStringAsync();
        Assert.Contains("dados inválidos", conteudo.ToLower());
    }

    [Fact]
    public async Task ComprarJogo_JogoNaoExiste_DeveRetornarNotFound()
    {
        // 1. Realiza login e obtém o token JWT
        var loginRequest = new
        {
            Email = "gustavo@rodrigues.com",
            Password = "12345abc@"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(loginContent);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();

        // 2. Adiciona o token no header Authorization
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        int idUsuario = 1;
        int idJogoInexistente = 9999;

        var response = await _client.PostAsync($"/api/Jogo/ComprarJogo?idUsuario={idUsuario}&idJogo={idJogoInexistente}", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var conteudo = await response.Content.ReadAsStringAsync();
        Assert.Contains("jogo não encontrado", conteudo.ToLower());
    }
}