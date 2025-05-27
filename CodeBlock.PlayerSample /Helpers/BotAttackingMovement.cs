using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Helpers;

public static class BotAttackingMovement
{
    // Cardinal direction movements with strikes
    public static Move MoveBotNorthAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y + 2)
        };
    }

    public static Move MoveBotNorthAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotNorthAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y + 1)
        };
    }

    public static Move MoveBotNorthAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y + 1)
        };
    }

    public static Move MoveBotSouthAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotSouthAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y - 2)
        };
    }

    public static Move MoveBotSouthAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y - 1)
        };
    }

    public static Move MoveBotSouthAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y - 1)
        };
    }

    public static Move MoveBotWestAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y + 1)
        };
    }

    public static Move MoveBotWestAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y - 1)
        };
    }

    public static Move MoveBotWestAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y)
        };
    }

    public static Move MoveBotWestAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotEastAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y + 1)
        };
    }

    public static Move MoveBotEastAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y - 1)
        };
    }

    public static Move MoveBotEastAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotEastAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y)
        };
    }

    // Diagonal movement methods with both diagonal and cardinal strike capabilities
    public static Move MoveBotNorthwestAndStrikeNorthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y + 2)
        };
    }

    public static Move MoveBotNorthwestAndStrikeNortheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y + 2)
        };
    }

    public static Move MoveBotNorthwestAndStrikeSouthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y)
        };
    }

    public static Move MoveBotNorthwestAndStrikeSoutheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    // Cardinal strikes from Northwest movement
    public static Move MoveBotNorthwestAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y + 2)
        };
    }

    public static Move MoveBotNorthwestAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y)
        };
    }

    public static Move MoveBotNorthwestAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y + 1)
        };
    }

    public static Move MoveBotNorthwestAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y + 1)
        };
    }

    public static Move MoveBotNortheastAndStrikeNorthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y + 2)
        };
    }

    public static Move MoveBotNortheastAndStrikeNortheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y + 2)
        };
    }

    public static Move MoveBotNortheastAndStrikeSouthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotNortheastAndStrikeSoutheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y)
        };
    }

    // Cardinal strikes from Northeast movement
    public static Move MoveBotNortheastAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y + 2)
        };
    }

    public static Move MoveBotNortheastAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y)
        };
    }

    public static Move MoveBotNortheastAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y + 1)
        };
    }

    public static Move MoveBotNortheastAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y + 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y + 1)
        };
    }

    public static Move MoveBotSouthwestAndStrikeNorthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y)
        };
    }

    public static Move MoveBotSouthwestAndStrikeNortheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotSouthwestAndStrikeSouthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y - 2)
        };
    }

    public static Move MoveBotSouthwestAndStrikeSoutheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y - 2)
        };
    }

    // Cardinal strikes from Southwest movement
    public static Move MoveBotSouthwestAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y)
        };
    }

    public static Move MoveBotSouthwestAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y - 2)
        };
    }

    public static Move MoveBotSouthwestAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y - 1)
        };
    }

    public static Move MoveBotSouthwestAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X - 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 2, position.Y - 1)
        };
    }

    public static Move MoveBotSoutheastAndStrikeNorthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y)
        };
    }

    public static Move MoveBotSoutheastAndStrikeNortheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y)
        };
    }

    public static Move MoveBotSoutheastAndStrikeSouthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y - 2)
        };
    }

    public static Move MoveBotSoutheastAndStrikeSoutheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y - 2)
        };
    }

    // Cardinal strikes from Southeast movement
    public static Move MoveBotSoutheastAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y)
        };
    }

    public static Move MoveBotSoutheastAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y - 2)
        };
    }

    public static Move MoveBotSoutheastAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 2, position.Y - 1)
        };
    }

    public static Move MoveBotSoutheastAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = new Position(position.X + 1, position.Y - 1),
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y - 1)
        };
    }

    // Stay in place methods with cardinal strikes
    public static Move StayAndStrikeNorth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y + 1)
        };
    }

    public static Move StayAndStrikeSouth(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X, position.Y - 1)
        };
    }

    public static Move StayAndStrikeWest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y)
        };
    }

    public static Move StayAndStrikeEast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y)
        };
    }

    // Stay in place methods with diagonal strikes
    public static Move StayAndStrikeNorthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y + 1)
        };
    }

    public static Move StayAndStrikeNortheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y + 1)
        };
    }

    public static Move StayAndStrikeSouthwest(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X - 1, position.Y - 1)
        };
    }

    public static Move StayAndStrikeSoutheast(this PlayerBot bot, Position position)
    {
        return new Move
        {
            Position = position,
            Action = MoveAction.Strike,
            TargetPosition = new Position(position.X + 1, position.Y - 1)
        };
    }
}