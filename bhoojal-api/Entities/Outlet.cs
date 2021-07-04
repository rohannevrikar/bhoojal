using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bhoojal.api
{
    public class Outlet
    {
    //     {
    //      "id": "8",
    // "city": "mumbai",
    // "quantity_score": 0,
    // "consumption_quota": 10000,
    // "email": "demo.bhoojal@hotmail.com",
    // "consumption_quota_available": 9054,
    // "quality_score": 0,
    // "quality_ph": 5,
    // "quality_hardness": 123,
    // "quality_updatedon": 1624632958,
    // "location": {
    //     "type": "Point",
    //     "coordinates": [
    //         72.832489,
    //         18.9553242
    //     ]
    // },
        public string Id { get; set; }
        public string City { get; set; }

        public float? Quality_Score { get; set; }
        public float? Quantity_Score { get; set; }
        public float? Consumption_Quota { get; set; }
        public float? Consumption_Quota_Available { get; set; }
        public float? Quality_Ph { get; set; }

        public float? quality_hardness {get; set;}
        public int?    Quality_UpdatedOn { get; set; }

        public string  Email { get; set; }
        public GPRLocation Location { get; set; }
    }

    public class GPRLocation
    {
        public string Type { get; set; }
        public List<float> Coordinates {get; set;}
    }
}
