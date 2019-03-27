using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DeviceTwins.Fabric.DeviceRegistry.Interfaces
{
    [DataContract]
    public class Device
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public DateTime RegisteredDate { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
