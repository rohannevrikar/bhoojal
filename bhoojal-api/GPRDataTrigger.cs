using System.Threading.Tasks;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;


namespace bhoojal.api
{
    public static class GPRDataTrigger
    {
        [FunctionName("GPRDataTrigger")]
        public static async Task Run([BlobTrigger("bhoojal/raw/{name}", Connection = "bhoojalstorageaccount_STORAGE")] Stream uploadedRawGPRData, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {uploadedRawGPRData.Length} Bytes");

            string ADB_AccessToken = Environment.GetEnvironmentVariable("ADB_AccessToken");
            string ADB_Endpoint = Environment.GetEnvironmentVariable("ADB_Endpoint");
            string ADB_GPR_JobId = Environment.GetEnvironmentVariable("ADB_GPR_JobId");
            string ADB_Polygon_JobId = Environment.GetEnvironmentVariable("ADB_Polygon_JobId");

            //TODO: Move to service
            string triggerAPIEndpoint = $"/api/2.0/jobs/run-now";
            DatabricksJob gpr_job = new DatabricksJob()
            {
                Job_Id = Convert.ToInt32(ADB_GPR_JobId),
                Notebook_Params = new NotebookParams()
                {
                    FileName = name
                }
            };
            DatabricksJob polygon_job = new DatabricksJob()
            {
                Job_Id = Convert.ToInt32(ADB_Polygon_JobId),
                Notebook_Params = new NotebookParams()
                {
                    FileName = name
                }
            };
            log.LogInformation("Triggering job");

            HttpClient client = new HttpClient();
            var gprRequest = new HttpRequestMessage(HttpMethod.Post, $"{ADB_Endpoint}{triggerAPIEndpoint}");
            gprRequest.Content = new StringContent(JsonConvert.SerializeObject(gpr_job), System.Text.Encoding.UTF8, "application/json");
            gprRequest.Headers.Add("Authorization", $"Bearer {ADB_AccessToken}");
            var gprResponse = await client.SendAsync(gprRequest);
            if (gprResponse.IsSuccessStatusCode)
            {
                log.LogInformation("GPR Job triggered");
                var content = gprResponse.Content;
                string jsonContent = content.ReadAsStringAsync().Result;
                log.LogInformation(jsonContent);
            }
            else
            {
                log.LogInformation($"GPR Job trigger API failed. Response code: {gprResponse.StatusCode}, reason: {gprResponse.ReasonPhrase}");
            }

            var polygonRequest = new HttpRequestMessage(HttpMethod.Post, $"{ADB_Endpoint}{triggerAPIEndpoint}");
            polygonRequest.Content = new StringContent(JsonConvert.SerializeObject(polygon_job), System.Text.Encoding.UTF8, "application/json");
            polygonRequest.Headers.Add("Authorization", $"Bearer {ADB_AccessToken}");
            var polygonResponse = await client.SendAsync(polygonRequest);
            if (polygonResponse.IsSuccessStatusCode)
            {
                log.LogInformation("Polygon Job triggered");
                var content = polygonResponse.Content;
                string jsonContent = content.ReadAsStringAsync().Result;
                log.LogInformation(jsonContent);
            }
            else
            {
                log.LogInformation($"Polygon Job trigger API failed. Response code: {polygonResponse.StatusCode}, reason: {polygonResponse.ReasonPhrase}");
            }
        }


    }
}
