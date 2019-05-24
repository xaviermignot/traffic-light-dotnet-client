using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using TrafficLight.Client.Abstractions;
using TrafficLight.Client.Core.Configuration;
using TrafficLight.Client.Core.Wrappers;

namespace TrafficLight.Client.Core
{
    public class IotHubTrafficLight
    {
        private IGpioClient _gpioClient;
        private IotHubConfiguration _configuration;
        private IDeviceClient _iotHubClient;
        private IDictionary<TrafficLightState, GpioLightMapping> _lightsGpioMappings = new Dictionary<TrafficLightState, GpioLightMapping>
        {
            { TrafficLightState.Green, GpioLightMapping.GreenLight },
            { TrafficLightState.Orange, GpioLightMapping.OrangeLight },
            { TrafficLightState.Red, GpioLightMapping.RedLight }
        };

        private TrafficLightState _currentState;

        public TrafficLightState CurrentState { get => _currentState; }

        public IotHubTrafficLight(IGpioClient gpioClient, IDeviceClient deviceClient, IIotHubConfigurationProvider configProvider)
        {
            _gpioClient = gpioClient;
            _iotHubClient = deviceClient;
            _configuration = configProvider.Get();

            InitGpio();
            InitDeviceClient();
        }

        private void InitGpio()
        {
            foreach (var pair in _lightsGpioMappings)
            {
                _gpioClient.OpenOutputPin((int)pair.Value);
            }

            _gpioClient.OpenInputPin((int)GpioLightMapping.Button, ButtonPressed);
        }

        private void InitDeviceClient()
        {
            _iotHubClient.CreateFromConnectionString(_configuration.ConnectionString, TransportType.Mqtt);
        }

        public void LightBulb(TrafficLightState lightValue)
        {
            foreach (var pair in _lightsGpioMappings)
            {
                _gpioClient.WritePin((int)pair.Value, pair.Key == lightValue ? GpioPinValue.High : GpioPinValue.Low);
            }

            _currentState = lightValue;
        }

        public void ButtonPressed()
        {
            if (_currentState == TrafficLightState.Red)
            {
                LightBulb(TrafficLightState.Off);
            }
            else
            {
                LightBulb(++_currentState);
            }
        }

        public async Task Startup()
        {
            foreach (var color in _lightsGpioMappings.Keys)
            {
                LightBulb(color);
                await Task.Delay(1000);
            }

            LightBulb(TrafficLightState.Off);
        }
    }
}
