using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.OpenApi.Extensions;

namespace bhoojal.api
{
    public static class Swagger
    {
        [FunctionName("Swagger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var input = new OpenApiGeneratorConfig(
       annotationXmlDocuments: new List<XDocument>()
       {
            XDocument.Load(@"AzureFunctionsOpenAPIDemo.xml"),
       },
       assemblyPaths: new List<string>()
       {
            @"bin\AzureFunctionsOpenAPIDemo.dll"
       },
       openApiDocumentVersion: "V1",
       filterSetVersion: FilterSetVersion.V1
   );
            input.OpenApiInfoDescription = "This is a sample description...";

            var generator = new OpenApiGenerator();
            var openApiDocuments = generator.GenerateDocuments(
                openApiGeneratorConfig: input,
                generationDiagnostic: out GenerationDiagnostic result
            );

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(openApiDocuments.First().Value.SerializeAsJson(OpenApiSpecVersion.OpenApi2_0), Encoding.UTF8, "application/json")
            };
        }
    }
}
