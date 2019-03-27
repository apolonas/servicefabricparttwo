using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeviceTwins.Fabric.DeviceRegistry.Interfaces;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace DeviceTwins.Fabric.DeviceRegistry
{
    public class DeviceRegistryRepository : IDeviceRegistryRepository
    {
        private readonly IReliableStateManager _stateManager;

        public DeviceRegistryRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddDevice(Device device)
        {
            var devices = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Device>>("devices");

            using (var tx = _stateManager.CreateTransaction())
            {
                await devices.AddOrUpdateAsync(tx, device.Id, device, (id, value) => device);
                await tx.CommitAsync();
            }
        }

        public async Task<Device> GetDevice(Guid deviceId)
        {
            var devices = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Device>>("devices");

            using (var tx = _stateManager.CreateTransaction())
            {
                ConditionalValue<Device> device = await devices.TryGetValueAsync(tx, deviceId);

                if (device.HasValue)
                {
                    return device.Value;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Device>> GetDevices()
        {
            var devices = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Device>>("devices");
            var result = new List<Device>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allDevices = await devices.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allDevices.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Device> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}
