using System.Net;
using System.Text.Json;
using CodeBlock.PlayerSample.Models;
using CodeBlock.PlayerSample.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CodeBlock.PlayerSample;

public class GameInitialisation(GamePersistence gamePersistence, ILogger<GameInitialisation> logger) : EndPoint
{
    [Function("GameInitialisation")]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        logger.LogInformation("Game initialization started");
        
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            logger.LogInformation("Request body read successfully, length: {BodyLength}", requestBody.Length);

            // Deserialize the JSON to BotTurnInput
            var gameInitialisationPayload = JsonSerializer.Deserialize<GameInitialisationPayload>(
                requestBody,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (gameInitialisationPayload == null)
            {
                logger.LogError("Failed to deserialize game initialization payload - payload is null");
                return;
            }
            
            logger.LogInformation("Setting up game with base URL: {BaseUrl}", gameInitialisationPayload.TheBaseUrl);
            logger.LogInformation("Player type: {PlayerType}", gameInitialisationPayload.YourType);

            gamePersistence.GameEndpointBaseUrl = gameInitialisationPayload.TheBaseUrl;
            gamePersistence.EndpointSecret = gameInitialisationPayload.YourSecretKey;
            gamePersistence.PlayerType = gameInitialisationPayload.YourType;

            logger.LogInformation("Game initialization completed successfully");
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize game initialization payload");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during game initialization");
        }
    }
}