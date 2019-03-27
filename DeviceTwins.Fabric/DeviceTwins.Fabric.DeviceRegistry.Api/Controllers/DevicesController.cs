using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceTwins.Fabric.DeviceRegistry.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;

namespace DeviceTwins.Fabric.DeviceRegistry.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRegistry _deviceRegistryService;

        public DevicesController()
        {
            var serviceProxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory(
                serializationProvider: new CustomDataContractProvider()));

            _deviceRegistryService = serviceProxyFactory.CreateServiceProxy<IDeviceRegistry>(
                new Uri("fabric:/DeviceTwins.Fabric/DeviceTwins.Fabric.DeviceRegistry"),
                new ServicePartitionKey(0), TargetReplicaSelector.PrimaryReplica);
        }

        [HttpGet]
        public async Task<IEnumerable<Models.Device>> Get()
        {
            IEnumerable<Interfaces.Device> devices = await _deviceRegistryService.GetDevices();

            return devices.OrderBy(d => d.RegisteredDate)
                            .Select(d => new Models.Device
                            {
                                Id = d.Id,
                                RegisteredDate = d.RegisteredDate,
                                Status = d.Status,
                                Type = d.Type
                            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Device>> Get(string id)
        {
            var device = await _deviceRegistryService.GetDevice(Guid.Parse(id));

            if (device == null)
                return NotFound();

            return new Models.Device()
            {
                Id = device.Id,
                RegisteredDate = device.RegisteredDate,
                Status = device.Status,
                Type = device.Type
            };
        }

        [HttpPost]
        public async Task Post([FromBody] Models.Device device)
        {
            var newDevice = new DeviceRegistry.Interfaces.Device()
            {
                Id = device.Id,
                RegisteredDate = device.RegisteredDate,
                Status = device.Status,
                Type = device.Type
            };

            await _deviceRegistryService.AddDevice(newDevice);
        }
    }
}