using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace bhoojal.api
{
    public static class GPRDataTrigger
    {
        [FunctionName("GPRDataTrigger")]
        public static void Run([BlobTrigger("gprdata/{name}", Connection = "bhoojalstorageaccount_STORAGE")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
