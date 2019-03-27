using DeviceTwins.Fabric.DeviceRegistry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTwins.Fabric.DeviceRegistry
{
    public interface IDeviceRegistryRepository
    {
        Task<IEnumerable<Device>> GetDevices();

        Task<Device> GetDevice(Guid deviceId);

        Task AddDevice(Device device);
    }
}
