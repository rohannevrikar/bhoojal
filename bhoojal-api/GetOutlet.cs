using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace bhoojal.api
{
    public static class GetOutlet
    {
        [FunctionName("GetOutlet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "outlet/{city}/{id}")] HttpRequest req,
            [CosmosDB(databaseName: "bhoojal_outlets", collectionName: "outlet", Id = "{id}",
                PartitionKey = "{city}", ConnectionStringSetting = "CosmosDBConnectionString")] Outlet outlet,
            ILogger log,
            string id)
        {
            if (outlet == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(outlet);
        }
    }
}
