using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTwins.DeviceSimulator.UI.Models
{
    public class Device
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string Status { get; set; }
    }
}
