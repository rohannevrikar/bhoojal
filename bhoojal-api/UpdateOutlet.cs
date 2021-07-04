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
using Microsoft.Azure.Cosmos;

namespace bhoojal.api
{
    public static class UpdateOutlet
    {
        private static CosmosClient cosmosClient;

        /// <summary>
        /// Update Outlet details
        /// </summary>
        /// <group>Get Outlet details V1</group>
        /// <verb>GET</verb>
        /// <url>https://bhoojal-api.azurewebsites.net/api/outlet/{city}/{id}</url>        /// 
        /// <param name="id" cref="string" in="path">The outlet id</param>
        /// <param  name="city" cref="string" in="path">The outlet city</param>
        /// <response code="200"><see cref="Outlet"/>Outlet</response>
        /// <response code="400"></response>
        /// <returns>String</returns>
        [FunctionName("UpdateOutlet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "outlet/{city}/{id}")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request.");

            string id = req.Query["id"];
            string city = req.Query["city"];
            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            Outlet data = JsonConvert.DeserializeObject<Outlet>(requestBody);

            id = id ?? data?.Id;
            string CosmosDBConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
            cosmosClient = new CosmosClient(CosmosDBConnectionString);
            var container = cosmosClient.GetContainer("bhoojal_outlets", "outlet");
            ItemResponse<Outlet> outletResponse = await container.ReadItemAsync<Outlet>(id, new PartitionKey(city));
            if (outletResponse == null || data == null)
            {
                return new BadRequestResult();
            }
            Outlet itemBody = outletResponse.Resource;

            // update registered email
            itemBody.Email = data.Email;

            // replace the item with the updated content
            outletResponse = await container.ReplaceItemAsync<Outlet>(itemBody, itemBody.Id, new PartitionKey(city));

            string responseMessage = string.IsNullOrEmpty(id)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Updated outlet {id} successfully";

            return new OkObjectResult(responseMessage);
        }
    }
}
