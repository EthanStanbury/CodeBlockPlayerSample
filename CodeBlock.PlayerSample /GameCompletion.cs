using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

namespace CodeBlock.PlayerSample;

public class GameCompletion
{
    [Function("GameCompletion")]
    public Task Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        return Task.CompletedTask;
    }

}