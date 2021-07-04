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
            string ADB_JobId = Environment.GetEnvironmentVariable("ADB_JobId");
            log.LogInformation($"{ADB_AccessToken}");

            //TODO: Move to service
            string triggerAPIEndpoint = $"/api/2.0/jobs/run-now";
            DatabricksJob job = new DatabricksJob()
            {
                Job_Id = Convert.ToInt32(ADB_JobId),
                Notebook_Params = new NotebookParams()
                {
                    FileName = name
                }
            };
            log.LogInformation("Triggering job");

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{ADB_Endpoint}{triggerAPIEndpoint}");
            request.Content = new StringContent(JsonConvert.SerializeObject(job), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {ADB_AccessToken}");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                log.LogInformation("Job triggered");
                var content = response.Content;
                string jsonContent = content.ReadAsStringAsync().Result;
                log.LogInformation(jsonContent);
            }
            else
            {
                log.LogInformation($"Job trigger API failed. Response code: {response.StatusCode}, reason: {response.ReasonPhrase}");
            }
        }


    }
}
