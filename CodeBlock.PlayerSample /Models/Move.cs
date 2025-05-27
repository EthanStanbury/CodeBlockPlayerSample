namespace CodeBlock.PlayerSample.Models;

public record Move
{
    public required MoveAction Action { get; set; }
    public required Position Position { get; set; }
    public Position? TargetPosition { get; set; }
    
    public BotType? NewBotType { get; set; }
}