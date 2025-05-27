namespace CodeBlock.PlayerSample.Models;

public class PlayerSquare
{
    public PlayerType SquarePlayerType { get; set; } = PlayerType.Neutral;

    public PlayerBot? OnSquare { get; set; }

    public Position XYPosition { get; set; } = new Position (0,0);
    
    public bool IsDefended {get; set;} = false;
    
    public PlayerType IsDefendedBy { get; set; } = PlayerType.Neutral;
    
    public int DefendedByTimer { get; set; } = 0;
}