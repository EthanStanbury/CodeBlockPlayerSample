namespace CodeBlock.PlayerSample.Models;

public struct PlayerTurnInput
{
    public Guid? TimeStamp { get; set; }

    public List<PlayerMoveSet>? TurnMoveSets { get; set; }
}