using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bhoojal.api
{
    public static class GetCities
    {
        [FunctionName("GetCities")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "cities")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get cities called");


            List<string> responseMessage = new List<string>() { "pune", "pcmc", "bmc" };
            return new OkObjectResult(responseMessage);
        }
    }
}
