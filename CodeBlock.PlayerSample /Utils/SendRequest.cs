using System.Text;
using System.Text.Json;
using CodeBlock.PlayerSample.Configuration;
using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Utils;

public static class SendRequest
{
    public static async Task SendEngineMessage<T>(
        T messageToSend, 
        HttpClient httpClient,
        GameEngineConfiguration gameEngineConfiguration) where T : GameEngineRequestBase
    {
        var json = JsonSerializer.Serialize(messageToSend);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, gameEngineConfiguration.WebsiteUrl + "/" + GetEndpointPath(messageToSend.Endpoint));
        request.Content = content;
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private static string GetEndpointPath(GameEngineEndpoint endpoint)
    {
        return endpoint switch
        {
            GameEngineEndpoint.SignUp => "api/signup",
            GameEngineEndpoint.RedInput => "api/red-input",
            GameEngineEndpoint.BlueInput => "api/blue-input",
            _ => throw new ArgumentOutOfRangeException(nameof(endpoint), endpoint, null)
        };
    }
}