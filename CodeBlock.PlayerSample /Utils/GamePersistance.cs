using CodeBlock.PlayerSample.Models;

namespace CodeBlock.PlayerSample.Utils;

public class GamePersistence
{
    public string GameEndpointBaseUrl = string.Empty;
    public Guid Bot1Id = Guid.NewGuid();
    public Guid Bot2Id = Guid.NewGuid();
    public Guid Bot3Id = Guid.NewGuid();
    public Guid EndpointSecret;
    public PlayerType PlayerType;
}
