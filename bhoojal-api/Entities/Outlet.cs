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
    public class Outlet
    {
        //        {
        //     "id": "1",
        //     "city": "pune",
        //     "quantity_score": -0.1,
        //     "consumption_quota": 10000,
        //     "consumption_quota_available": 10000,
        //     "quality_score": -0.1,
        //     "quality_ph": -0.1,
        //     "quality_hardness": -0.1,
        //     "quality_updatedon": "",
        //     "_rid": "QJplAIr5B+MBAAAAAAAAAA==",
        //     "_self": "dbs/QJplAA==/colls/QJplAIr5B+M=/docs/QJplAIr5B+MBAAAAAAAAAA==/",
        //     "_etag": "\"a20051c6-0000-0d00-0000-60d5b60c0000\"",
        //     "_attachments": "attachments/",
        //     "_ts": 1624618508
        // }
        public string Id { get; set; }
        public string City { get; set; }

        public float? Quality_Score { get; set; }
        public float? Quantity_Score { get; set; }
        public float? Consumption_Quota { get; set; }
        public float? Consumption_Quota_Available { get; set; }
        public float? Quality_Ph { get; set; }
        public DateTime?    Quality_UpdatedOn { get; set; }
    }
}
