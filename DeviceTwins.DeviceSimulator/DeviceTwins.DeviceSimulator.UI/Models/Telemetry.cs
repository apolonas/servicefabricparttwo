using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTwins.DeviceSimulator.UI.Models
{
    public class Telemetry
    {
        public DateTime Timestamp { get; set; }

        public float Temperature { get; set; }

        public float Humidity { get; set; }
    }
}
