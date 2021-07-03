// {
//   "job_id": 1,
//   "notebook_params": {
//     "name": "john doe",
//     "age": "35"
//   }
// }

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
    public class DatabricksJob
    {
        //"job_id": 1,
        //   "notebook_params": {
        //     "name": "john doe",
        //     "age": "35"
        //   }
        [JsonProperty("job_id")]
        public int Job_Id { get; set; }
          [JsonProperty("notebook_params")]
        public NotebookParams Notebook_Params { get; set; }
    }

    public class NotebookParams
    {
        public string FileName { get; set; }
    }
}
