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
        /// <summary>
        /// Get Outlet details
        /// </summary>
        /// <group>Get Outlet details V1</group>
        /// <verb>GET</verb>
        /// <url>https://bhoojal-api.azurewebsites.net/api/outlet/{city}/{id}</url>        /// 
        /// <param name="id" cref="string" in="path">The outlet id</param>
        /// <param  name="city" cref="string" in="path">The outlet city</param>
        /// <response code="200"><see cref="Outlet"/>Outlet</response>
        /// <response code="400"></response>
        /// <returns>Outlet</returns>
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
