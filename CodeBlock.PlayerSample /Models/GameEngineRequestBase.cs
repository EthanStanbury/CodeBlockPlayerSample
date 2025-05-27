namespace CodeBlock.PlayerSample.Models;

public abstract record GameEngineRequestBase
{
    public abstract GameEngineEndpoint Endpoint { get; }
}