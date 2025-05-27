using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Helpers;

public static class BotSelection
{
    public static PlayerSquare? FindBot(this PlayerBoard board, Guid botId)
    {
        return board.BoardGrid
            .SelectMany(column => column)
            .First(square => square.OnSquare is not null && 
                             square.OnSquare.BotId == botId);
    }

    public static List<PlayerSquare> FindAllBots(this PlayerBoard board, List<Guid> botIds)
    {
        return board.BoardGrid
            .SelectMany(column => column)
            .Where(square => square.OnSquare is not null && 
                             botIds.Contains(square!.OnSquare.BotId))
            .ToList();
    }
}