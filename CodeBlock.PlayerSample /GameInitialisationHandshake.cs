using System.Net;
using System.Text.Json;
using CodeBlock.PlayerSample.Models;
using CodeBlock.PlayerSample.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
namespace CodeBlock.PlayerSample;

public class GameInitialisationHandshake(GamePersistence gamePersistence, ILogger<GameInitialisationHandshake> logger) : EndPoint
{
    [Function("GameInitialisationHandshake")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
        FunctionContext executionContext)
    {
        // Deserialize the JSON to BotTurnInput
        var response = new PlayerBotInformationExchange()
        {
            FirstPlayerBot = new()
            {
                BotId = gamePersistence.Bot1Id,
                Type = BotType.Painter
            },
            SecondPlayerBot = new()
            {
                BotId = gamePersistence.Bot2Id,
                Type = BotType.Defense
            },
            ThirdPlayerBot = new()
            {
                BotId = gamePersistence.Bot3Id,
                Type = BotType.Attack
            }
        };
        
        logger.LogInformation(
            "Attempting to initialise handshake bots. Bot IDs: {Bot1Id}, {Bot2Id}, {Bot3Id}",
            gamePersistence.Bot1Id,
            gamePersistence.Bot2Id,
            gamePersistence.Bot3Id
        );
        
        var httpResponse = req.CreateResponse(HttpStatusCode.OK);
        httpResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
    
        var jsonResponse = JsonSerializer.Serialize(response);
        httpResponse.WriteString(jsonResponse);
        
        logger.LogInformation(
            "Successfully initialised handshake bots. Bot IDs: {Bot1Id}, {Bot2Id}, {Bot3Id}",
            gamePersistence.Bot1Id,
            gamePersistence.Bot2Id,
            gamePersistence.Bot3Id
        );
        
        return httpResponse;
    }
}