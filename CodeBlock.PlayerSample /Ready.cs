using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CodeBlock.PlayerSample
{
    public class Ready(ILogger<Ready> logger)
    {
        [Function("Ready")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            try
            {
                logger.LogDebug("Received ready check request");
                return new OkResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing ready check request");
                return new StatusCodeResult(503); 
            }
        }
    }
}