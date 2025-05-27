using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Helpers;

public static class BotPaintingMovement
{
    // Cardinal direction movements
    public static Move MoveBotNorthAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Paint
        };
    }

    public static Move MoveBotSouthAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Paint
        };
    }

    public static Move MoveBotWestAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Paint
        };
    }

    public static Move MoveBotEastAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Paint
        };
    }

    // Diagonal movements
    public static Move MoveBotNorthwestAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Paint
        };
    }

    public static Move MoveBotNortheastAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Paint
        };
    }

    public static Move MoveBotSouthwestAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Paint
        };
    }

    public static Move MoveBotSoutheastAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Paint
        };
    }

    public static Move StayAndPaint(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Paint
        };
    }
}