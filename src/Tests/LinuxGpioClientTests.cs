using System.Device.Gpio;
using Moq;
using TrafficLight.Client.Abstractions;
using TrafficLight.Client.Gpio;
using TrafficLight.Client.Gpio.Wrappers;
using Xunit;

namespace TrafficLight.Client.Tests
{
    public class LinuxGpioClientTests
    {
        private Mock<Gpio.Wrappers.IGpioController> _fakeGpioController = new Mock<Gpio.Wrappers.IGpioController>();
        private LinuxGpioClient _client;

        public LinuxGpioClientTests()
        {
            _client = new LinuxGpioClient(_fakeGpioController.Object);
        }

        [Fact]
        public void OpenOutputPinShouldUseGpioController()
        {
            // Act
            _client.OpenOutputPin(42);

            // Assert
            _fakeGpioController.Verify(c => c.OpenPin(42, PinMode.Output), Times.Once);
        }

        [Fact]
        public void OpenInputPinShouldUseGpioController()
        {
            // Act
            _client.OpenInputPin(42, null);

            // Assert
            _fakeGpioController.Verify(c => c.OpenPin(42, PinMode.InputPullUp), Times.Once());
        }

        [Fact]
        public void OpenInputPinShouldRegisterCallback()
        {
            // Act
            _client.OpenInputPin(42, null);

            // Assert
            _fakeGpioController.Verify(
                c => c.RegisterCallbackForPinValueChangedEvent(
                    42,
                    PinEventTypes.Rising,
                    It.IsAny<PinChangeEventHandler>()));
        }

        [Fact]
        public void WritePinShouldUseGpioController()
        {
            // Act
            _client.WritePin(42, GpioPinValue.High);
            _client.WritePin(51, GpioPinValue.Low);

            // Assert
            _fakeGpioController.Verify(c => c.Write(42, PinValue.High), Times.Once());
            _fakeGpioController.Verify(c => c.Write(51, PinValue.Low), Times.Once());
        }
    }
}