using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Helpers;

public static class BotDefendingMovement
{
    // Cardinal direction movements
    public static Move MoveBotNorthAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Defend
        };
    }

    public static Move MoveBotSouthAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Defend
        };
    }

    public static Move MoveBotWestAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Defend
        };
    }

    public static Move MoveBotEastAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Defend
        };
    }

    // Diagonal movements
    public static Move MoveBotNorthwestAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Defend
        };
    }

    public static Move MoveBotNortheastAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Defend
        };
    }

    public static Move MoveBotSouthwestAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Defend
        };
    }

    public static Move MoveBotSoutheastAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Defend
        };
    }

    public static Move StayAndDefend(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Defend
        };
    }
}