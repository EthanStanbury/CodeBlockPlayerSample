namespace CodeBlock.PlayerSample.Models;

public class GameInitialisationPayload
{
    public PlayerType YourType { get; set; } = PlayerType.Neutral;
    public string TheBaseUrl { get; set; } = string.Empty;

    public Guid YourSecretKey { get; set; } = Guid.Empty;
}