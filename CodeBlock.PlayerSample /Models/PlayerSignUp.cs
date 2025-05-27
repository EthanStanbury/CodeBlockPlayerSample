namespace CodeBlock.PlayerSample.Models;

public record PlayerSignUp : GameEngineRequestBase
{
    public required string PlayersBaseUrl { get; set; }

    public required string PlayersTeamName  { get; set; }
    
    public override GameEngineEndpoint Endpoint { get;  } = GameEngineEndpoint.SignUp;
}