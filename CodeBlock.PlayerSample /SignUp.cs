using CodeBlock.PlayerSample.Configuration;
using CodeBlock.PlayerSample.Models;
using CodeBlock.PlayerSample.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeBlock.PlayerSample;

public class SignUp(
    IHttpClientFactory httpClientFactory,
    IOptions<GameEngineConfiguration> gameEngineConfiguration,
    IOptions<BotConfiguration> botConfiguration,
    ILogger<SignUp> logger)
{
    [Function("SignUp")]
    public async Task Run([TimerTrigger("*/5 */5 * * * *")] TimerInfo myTimer)
    {
        logger.LogInformation("Starting player sign-up process");
        
        try
        {
            var signUpMessage = new PlayerSignUp()
            {
                PlayersBaseUrl = botConfiguration.Value.BaseUrl,
                PlayersTeamName = botConfiguration.Value.TeamName,
            };
            
            logger.LogDebug("Sending sign-up request for team: {TeamName}", botConfiguration.Value.TeamName);
            
            await SendRequest.SendEngineMessage(signUpMessage, httpClientFactory.CreateClient(), gameEngineConfiguration.Value);
            
            logger.LogInformation("Player sign-up completed successfully");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Failed to send sign-up request to game engine");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during player sign-up");
        }
    }
}