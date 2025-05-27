namespace CodeBlock.PlayerSample.Models;

public record PlayerBotInformationExchange
{
    public PlayerBot FirstPlayerBot { get; set; } = new PlayerBot();
    
    public PlayerBot SecondPlayerBot { get; set; } = new PlayerBot();
    
    public PlayerBot ThirdPlayerBot { get; set; } = new PlayerBot();
}