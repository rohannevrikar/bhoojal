using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace bhoojal.api
{
    public static class UpdateOutlet
    {
        [FunctionName("UpdateOutlet")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "outlet/{city}/{id}")] HttpRequest req,
               [CosmosDB(databaseName: "bhoojal_outlets", collectionName: "outlet", Id = "{id}",
                PartitionKey = "{city}", ConnectionStringSetting = "CosmosDBConnectionString")] Outlet outletToBeUpdated,
                  [CosmosDB(databaseName: "bhoojal_outlets", collectionName: "outlet", Id = "{id}",
                PartitionKey = "{city}", ConnectionStringSetting = "CosmosDBConnectionString")] out  Outlet updatedOutlet,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request.");

            string id = req.Query["id"];
            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            Outlet data = JsonConvert.DeserializeObject<Outlet>(requestBody);

            id = id ?? data?.Id;


            if (outletToBeUpdated == null || data == null)
            {
                updatedOutlet = outletToBeUpdated;
                return new BadRequestResult();
            }

            updatedOutlet = data;

            string responseMessage = string.IsNullOrEmpty(id)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Updated outlet {id} successfully";

            return new OkObjectResult(responseMessage);
        }
    }
}
