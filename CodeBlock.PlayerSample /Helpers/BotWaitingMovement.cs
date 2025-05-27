using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Helpers;

public static class BotWaitingMovement
{
    // Cardinal direction movements
    public static Move MoveBotNorthAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Wait
        };
    }

    public static Move MoveBotSouthAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Wait
        };
    }

    public static Move MoveBotWestAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Wait
        };
    }

    public static Move MoveBotEastAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Wait
        };
    }

    // Diagonal movements
    public static Move MoveBotNorthwestAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Wait
        };
    }

    public static Move MoveBotNortheastAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Wait
        };
    }

    public static Move MoveBotSouthwestAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Wait
        };
    }

    public static Move MoveBotSoutheastAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Wait
        };
    }

    public static Move StayAndWait(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Wait
        };
    }
}