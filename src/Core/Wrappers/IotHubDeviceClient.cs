using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using TrafficLight.Client.Core.Wrappers;

namespace Core.Wrappers
{
    public class IotHubDeviceClient : IDeviceClient
    {
        private DeviceClient _real;

        public void CreateFromConnectionString(string connectionString, TransportType transportType)
        {
            _real = DeviceClient.CreateFromConnectionString(connectionString, transportType);
        }

        public Task UpdateReportedPropertiesAsync(TwinCollection twins)
        {
            return _real.UpdateReportedPropertiesAsync(twins);
        }
    }
}