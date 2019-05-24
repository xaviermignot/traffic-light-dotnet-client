using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Moq;
using TrafficLight.Client.Abstractions;
using TrafficLight.Client.Core;
using TrafficLight.Client.Core.Configuration;
using TrafficLight.Client.Core.Wrappers;
using Xunit;

namespace TrafficLight.Client.Tests
{
    public class IotHubTrafficLightTests
    {
        private Mock<IGpioClient> _fakeGpioClient = new Mock<IGpioClient>();
        private Mock<IDeviceClient> _fakeIotHubClient = new Mock<IDeviceClient>();
        private Mock<IIotHubConfigurationProvider> _fakeConfigurationProvider = new Mock<IIotHubConfigurationProvider>();
        private IotHubTrafficLight _trafficLight;

        public IotHubTrafficLightTests()
        {
            _fakeConfigurationProvider.Setup(p => p.Get()).Returns(new IotHubConfiguration());
            _trafficLight = new IotHubTrafficLight(_fakeGpioClient.Object, _fakeIotHubClient.Object, _fakeConfigurationProvider.Object);
        }

        [Fact]
        public void ButtonPressedShouldGoFromRedToOff()
        {
            // Arrange
            _trafficLight.LightBulb(TrafficLightState.Red);

            // Act
            _trafficLight.ButtonPressed();

            // Assert
            Assert.Equal(TrafficLightState.Off, _trafficLight.CurrentState);
        }

        [Fact]
        public void ButtonPressedShouldGoFromGreenToOrange()
        {
            // Arrange
            _trafficLight.LightBulb(TrafficLightState.Green);

            // Act
            _trafficLight.ButtonPressed();

            // Assert
            Assert.Equal(TrafficLightState.Orange, _trafficLight.CurrentState);
        }

        [Fact]
        public void LightBulbShouldLightOneBulb()
        {
            // Arrange
            _trafficLight.LightBulb(TrafficLightState.Red);

            // Act
            _trafficLight.LightBulb(TrafficLightState.Green);

            // Assert
            _fakeGpioClient.Verify(c => c.WritePin(27, GpioPinValue.High), Times.Once());
        }

        [Fact]
        public void GpioClientShouldOpenAllPins()
        {
            // Assert
            _fakeGpioClient.Verify(c => c.OpenOutputPin((int)GpioLightMapping.GreenLight), Times.Once());
            _fakeGpioClient.Verify(c => c.OpenOutputPin((int)GpioLightMapping.OrangeLight), Times.Once());
            _fakeGpioClient.Verify(c => c.OpenOutputPin((int)GpioLightMapping.RedLight), Times.Once());
            _fakeGpioClient.Verify(c => c.OpenInputPin((int)GpioLightMapping.Button, It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public async Task StartupShouldLightBulbsInOrder()
        {
            // Arrange
            _trafficLight.LightBulb(TrafficLightState.Off);

            // Act
            await _trafficLight.Startup();

            // Assert
            _fakeGpioClient.Verify(c => c.WritePin((int)GpioLightMapping.GreenLight, GpioPinValue.High), Times.Once());
            _fakeGpioClient.Verify(c => c.WritePin((int)GpioLightMapping.OrangeLight, GpioPinValue.High), Times.Once());
            _fakeGpioClient.Verify(c => c.WritePin((int)GpioLightMapping.RedLight, GpioPinValue.High), Times.Once());
            Assert.Equal(TrafficLightState.Off, _trafficLight.CurrentState);
        }

        [Fact]
        public void DeviceClientShouldBeInitializedWithConnectionString()
        {
            // Assert
            _fakeIotHubClient.Verify(c => c.CreateFromConnectionString(It.IsAny<string>(), TransportType.Mqtt), Times.Once());
            _fakeConfigurationProvider.Verify(p => p.Get(), Times.Once());
        }
    }
}
