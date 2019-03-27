using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTwins.Fabric.DeviceRegistry.Interfaces
{
    public interface IDeviceRegistry : IService
    {
        Task<IEnumerable<Device>> GetDevices();

        Task AddDevice(Device device);

        Task<Device> GetDevice(Guid deviceId);
    }
}
