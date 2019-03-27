using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceTwins.Fabric.DeviceActor.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace DeviceTwins.Fabric.DeviceRegistry.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        [HttpGet("{deviceId}")]
        public async Task<Models.Telemetry> Get(string deviceId)
        {
            IDeviceActor actor = GetActor(deviceId);
            DeviceActor.Interfaces.Telemetry telemetry = await actor.GetTelemetry();

            return new Models.Telemetry()
            {
                Humidity = telemetry.Humidity,
                Temperature = telemetry.Temperature,
                Timestamp = telemetry.Timestamp
            };
        }

        [HttpPost("{deviceId}")]
        public async Task Post(string deviceId, [FromBody] Models.Telemetry telemetry)
        {
            IDeviceActor actor = GetActor(deviceId);

            var deviceTelemetry = new DeviceActor.Interfaces.Telemetry()
            {
                Humidity = telemetry.Humidity,
                Temperature = telemetry.Temperature,
                Timestamp = telemetry.Timestamp
            };

            await actor.SaveTelemetry(deviceTelemetry);
        }

        private IDeviceActor GetActor(string deviceId)
        {
            return ActorProxy.Create<IDeviceActor>(
                new ActorId(deviceId),
                new Uri("fabric:/DeviceTwins.Fabric/DeviceActorService"));
        }
    }
}