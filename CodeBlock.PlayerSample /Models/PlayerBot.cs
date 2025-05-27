namespace CodeBlock.PlayerSample.Models;

public class PlayerBot
{
    public Guid BotId { get; set; } = Guid.Empty;

    public BotType Type { get; set; } = BotType.Empty;
    
    public BotStatus Status { get; set; } = new BotStatus();
    
    public int BotIncapacitatedTimer  { get; set; } = 0;

    public PlayerType TeamPlayerType { get; set; } = PlayerType.Neutral;
}