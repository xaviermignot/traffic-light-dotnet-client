using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace TrafficLight.Client.Core.Wrappers
{
    public interface IDeviceClient
    {
        void CreateFromConnectionString(string connectionString, TransportType transportType);
        Task UpdateReportedPropertiesAsync(TwinCollection twins);
    }
}