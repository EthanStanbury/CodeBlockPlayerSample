using System.Net;
using System.Text;
using System.Text.Json;
using CodeBlock.PlayerSample.Helpers;
using CodeBlock.PlayerSample.Models;
using CodeBlock.PlayerSample.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CodeBlock.PlayerSample;

public class BoardUpdate(
    IHttpClientFactory httpClientFactory,
    GamePersistence gamePersistence,
    ILogger<BoardUpdate> logger)
{
    [Function("BoardUpdate")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        var board = JsonSerializer.Deserialize<BoardStateUpdateMessage>(
            requestBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


        // TODO - Write your bot logic here.
        // This is the new game board from the game engine, after the opposition's turn.
        var updatedBoard = board!.CurrentBoardState;

        // This is the list of moves for each turn, you need to supply one for each bot. 
        var listOfMovesForTurn = new List<PlayerMoveSet>
        {
            MoveFirstBot(updatedBoard),
            MoveSecondBot(updatedBoard),
            MoveThirdBot(updatedBoard)
        };

        await SendEngineNewMove(new PlayerTurnInput()
        {
            TimeStamp = board.NewTurnTimestamp,
            TurnMoveSets = listOfMovesForTurn
        });
        logger.LogInformation($"Board update request sent: {updatedBoard}");
        return req.CreateResponse(HttpStatusCode.Accepted);
    }

    private async Task SendEngineNewMove(PlayerTurnInput listOfMovesForTurn)
    {
        try
        {
            var json = JsonSerializer.Serialize(listOfMovesForTurn);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Add Basic Authentication
            string base64Auth =
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        $"{gamePersistence.PlayerType}:{gamePersistence.EndpointSecret}"));
            using var request = new HttpRequestMessage(HttpMethod.Post,
                gamePersistence.GameEndpointBaseUrl + $"/api/{gamePersistence.PlayerType}Input");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);
            request.Content = content;
            var response = await httpClientFactory.CreateClient().SendAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    logger.LogError("Request sent to engine wasn't a valid move, the response was: {response}",
                        await response.Content.ReadAsStringAsync());
                    return;

                case var status when !response.IsSuccessStatusCode:
                    logger.LogError("Request sent to engine wasn't a valid move, the response was: {response}, and a status code of: {status}",
                        await response.Content.ReadAsStringAsync(), status);
                    break;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling Azure Function: {ex.Message}");
        }
    }

    /*
        Bot Movement Pattern: Along top row, then down right side

        10x10 Grid showing bot path (numbers show movement sequence):

           0 1 2 3 4 5 6 7 8 9
        0  1-2-3-4-5-6-7-8-9-10
        1  . . . . . . . . . 11
        2  . . . . . . . . . 12
        3  . . . . . . . . . 13
        4  . . . . . . . . . 14
        5  . . . . . . . . . 15
        6  . . . . . . . . . 16
        7  . . . . . . . . . 17
        8  . . . . . . . . . 18
        9  . . . . . . . . . 19

        Movement explanation:
        - Moves 1-10: Bot travels across top row from left to right (0,0) to (0,9)
        - Moves 11-19: Bot travels down right column from (1,9) to (9,9)
        - The dashes (-) show horizontal movement along the top
        - The dots (.) represent unvisited grid positions
        - Numbers show the sequence of bot positions
    */
    private PlayerMoveSet MoveFirstBot(PlayerBoard updatedBoard)
    {
        // Find your bot on the board, currently this is (O)n^2, you may wish to find a more efficient method.
        var firstBotSquare = updatedBoard.FindBot(gamePersistence.Bot1Id);
        if (firstBotSquare is null)
        {
            logger.LogError("Couldn't find the first bot: {botId}", gamePersistence.Bot1Id);
            throw new Exception($"Couldn't find the second bot id {gamePersistence.Bot1Id}");
        }

        var firstBotFirstMove = firstBotSquare.XYPosition.X switch
        {
            9 => firstBotSquare.OnSquare!.MoveBotSouthAndPaint(firstBotSquare.XYPosition),
            _ => firstBotSquare.OnSquare!.MoveBotEastAndPaint(firstBotSquare.XYPosition)
        };

        var firstBotSecondMove = firstBotFirstMove.Position.X switch
        {
            9 => firstBotSquare.OnSquare!.MoveBotSouthAndPaint(firstBotSquare.XYPosition),
            _ => firstBotSquare.OnSquare!.MoveBotEastAndPaint(firstBotSquare.XYPosition)
        };

        var firstBotThirdMove = firstBotSecondMove.Position.X switch
        {
            9 => firstBotSquare.OnSquare!.MoveBotSouthAndPaint(firstBotSquare.XYPosition),
            _ => firstBotSquare.OnSquare!.MoveBotEastAndPaint(firstBotSquare.XYPosition)
        };

        return new PlayerMoveSet()
        {
            PlayerBot = firstBotSquare.OnSquare!,
            Moves =
            [
                firstBotFirstMove,
                firstBotSecondMove,
                firstBotThirdMove
            ]
        };
    }

    /*
        Bot Movement Pattern: Diagonal southeast movement with varied attacks

        10x10 Grid showing bot path and attack targets:

           0 1 2 3 4 5 6 7 8 9
        0  S . . . . . . . . .
        1  . 1 X . . . . . . .
        2  X . 2 X . . . . . .
        3  . . . 3 . . . . . .
        4  . . . X 4 X . . . .
        5  . . X . . 5 . . . .
        6  . . . . . . 6 . . .
        7  . . . . . X . 7 X .
        8  . . . . . . . X 8 .
        9  . . . . . . . . . 9

        Legend:
        S = Starting position
        1,2,3... = Bot's position after each move
        X = Attack target locations
        . = Empty/unvisited positions

        Attack Pattern Cycle (repeats every 3 moves):
        - Moves 1,4,7: Attack EAST (→) - targets position to the right
        - Moves 2,5,8: Attack WEST (←) - targets position to the left
        - Moves 3,6,9: Attack SOUTH (↓) - targets position below
    */

    private PlayerMoveSet MoveSecondBot(PlayerBoard updatedBoard)
    {
        var secondBotSquare = updatedBoard.FindBot(gamePersistence.Bot2Id);
        if (secondBotSquare == null)
        {
            logger.LogError("Couldn't find the first bot: {botId}", gamePersistence.Bot1Id);
            throw new Exception($"Couldn't find the second bot id {gamePersistence.Bot2Id}");
        }

        var secondBotFirstMove = secondBotSquare.OnSquare!.MoveBotSoutheastAndStrikeEast(secondBotSquare.XYPosition);
        var secondBotSecondMove = secondBotSquare.OnSquare!.MoveBotSoutheastAndStrikeWest(secondBotSquare.XYPosition);
        var secondBotThirdMove = secondBotSquare.OnSquare!.MoveBotSoutheastAndStrikeSouth(secondBotSquare.XYPosition);

        return new PlayerMoveSet
        {
            PlayerBot = secondBotSquare.OnSquare!,
            Moves =
            [
                secondBotFirstMove,
                secondBotSecondMove,
                secondBotThirdMove
            ]
        };
    }

    /*
        Bot Movement Pattern: Down left column, then across bottom row

        10x10 Grid showing bot path (numbers show movement sequence):

           0  1  2  3  4  5  6  7  8  9
        0  1  D  .  .  .  .  .  .  .  .
        1  2  D  .  .  .  .  .  .  .  .
        2  3  D  .  .  .  .  .  .  .  .
        3  4  D  .  .  .  .  .  .  .  .
        4  5  D  .  .  .  .  .  .  .  .
        5  6  D  .  .  .  .  .  .  .  .
        6  7  D  .  .  .  .  .  .  .  .
        7  8  D  .  .  .  .  .  .  .  .
        8  9  D  D  D  D  D  D  D  D  D
        9  10-11-12-13-14-15-16-17-18-19

        Legend:
           D = Defended Square

        Movement explanation:
        - Moves 1-10: Bot travels down left column from top to bottom (0,0) to (9,0)
        - Moves 11-19: Bot travels across bottom row from left to right (9,1) to (9,9)
        - The dashes (-) show horizontal movement along the bottom
        - The dots (.) represent unvisited grid positions
        - Numbers show the sequence of bot positions

        Movement explanation:
        - Moves 1-10: Bot travels down left column from top to bottom (0,0) to (9,0)
        - Moves 11-19: Bot travels across bottom row from left to right (9,1) to (9,9)
        - The dashes (-) show horizontal movement along the bottom
        - The dots (.) represent unvisited grid positions
        - Numbers show the sequence of bot positions
    */
    private PlayerMoveSet MoveThirdBot(PlayerBoard updatedBoard)
    {
        // Find your bot on the board, currently this is (O)n^2, you may wish to find a more efficient method.
        var firstBotSquare = updatedBoard.FindBot(gamePersistence.Bot1Id);
        if (firstBotSquare is null)
        {
            logger.LogError("Couldn't find the first bot: {botId}", gamePersistence.Bot1Id);
            throw new Exception($"Couldn't find the second bot id {gamePersistence.Bot1Id}");
        }

        var firstBotFirstMove = firstBotSquare.XYPosition.Y switch
        {
            9 => firstBotSquare.OnSquare!.MoveBotEastAndDefend(firstBotSquare.XYPosition),
            _ => firstBotSquare.OnSquare!.MoveBotSouthAndDefend(firstBotSquare.XYPosition)
        };

        var firstBotSecondMove = firstBotFirstMove.Position.Y switch
        {
            9 => firstBotSquare.OnSquare!.MoveBotEastAndDefend(firstBotSquare.XYPosition),
            _ => firstBotSquare.OnSquare!.MoveBotSouthAndDefend(firstBotSquare.XYPosition)
        };

        var firstBotThirdMove = firstBotSecondMove.Position.Y switch
        {
            9 => firstBotSquare.OnSquare!.MoveBotEastAndDefend(firstBotSquare.XYPosition),
            _ => firstBotSquare.OnSquare!.MoveBotSouthAndDefend(firstBotSquare.XYPosition)
        };

        return new PlayerMoveSet()
        {
            PlayerBot = firstBotSquare.OnSquare!,
            Moves =
            [
                firstBotFirstMove,
                firstBotSecondMove,
                firstBotThirdMove
            ]
        };
    }
}
