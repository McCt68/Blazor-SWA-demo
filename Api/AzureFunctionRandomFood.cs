using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class AzureFunctionRandomFood
    {
        private readonly ILogger<AzureFunctionRandomFood> _logger;

        public AzureFunctionRandomFood(ILogger<AzureFunctionRandomFood> logger)
        {
            _logger = logger;
        }

        [Function("Function")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
