using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeviceTwins.Fabric.DeviceRegistry.Interfaces;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

[assembly: FabricTransportServiceRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2, RemotingClientVersion = RemotingClientVersion.V2)]
namespace DeviceTwins.Fabric.DeviceRegistry
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class DeviceRegistry : StatefulService, IDeviceRegistry
    {
        private IDeviceRegistryRepository _repository;

        public DeviceRegistry(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddDevice(Device device)
        {
            await _repository.AddDevice(device);
        }

        public async Task<Device> GetDevice(Guid deviceId)
        {
            return await _repository.GetDevice(deviceId);
        }

        public async Task<IEnumerable<Device>> GetDevices()
        {
            return await _repository.GetDevices();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener((context) =>
                {
                    return new FabricTransportServiceRemotingListener(context, this,
                        serializationProvider: new CustomDataContractProvider());
                })
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _repository = new DeviceRegistryRepository(this.StateManager); 
        }
    }
}
