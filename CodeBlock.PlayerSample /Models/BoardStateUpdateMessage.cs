namespace CodeBlock.PlayerSample.Models;

public class BoardStateUpdateMessage
{
    public Guid NewTurnTimestamp {get; set;} = Guid.Empty;
    
    public PlayerBoard CurrentBoardState { get; set; } = new PlayerBoard();
}