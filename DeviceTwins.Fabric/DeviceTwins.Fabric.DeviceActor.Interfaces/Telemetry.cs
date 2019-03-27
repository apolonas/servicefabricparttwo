using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DeviceTwins.Fabric.DeviceActor.Interfaces
{
    [DataContract]
    public class Telemetry
    {
        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public float Temperature { get; set; }

        [DataMember]
        public float Humidity { get; set; }
    }
}
