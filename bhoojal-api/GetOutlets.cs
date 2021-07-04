using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;

namespace bhoojal.api
{
    public static class GetOutlets
    {
         /// <summary>
        /// Get Outlets List
        /// </summary>
        /// <group>Get Outlets List V1</group>
        /// <verb>GET</verb>
        /// <url>https://bhoojal-api.azurewebsites.net/api/outlet/{city}/{id}</url>       
        /// <response code="200"><see cref="Outlet"/>Outlet list</response>
        /// <response code="400"></response>
        /// <returns>Outlet</returns>
        [FunctionName("GetOutlets")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "outlets")] HttpRequest req,
            [CosmosDB(databaseName: "bhoojal_outlets", collectionName: "outlet", SqlQuery = "SELECT top 100 * FROM outlet order by outlet._ts desc", ConnectionStringSetting = "CosmosDBConnectionString")] IEnumerable<Outlet> outlets,
    ILogger log)
        {
            log.LogInformation("Get outlets request received.");

            if (outlets is null)
            {
                return new NotFoundResult();
            }

            foreach (var outlet in outlets)
            {
                log.LogInformation($"{outlet.Id} {outlet.City}");
            }

            return new OkObjectResult(outlets);
        }
    }
}
