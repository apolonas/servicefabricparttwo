using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceTwins.Fabric.DeviceRegistry.Api.Models
{
    public class Telemetry
    {
        [JsonProperty]
        public DateTime Timestamp { get; set; }

        [JsonProperty]
        public float Temperature { get; set; }

        [JsonProperty]
        public float Humidity { get; set; }
    }
}
