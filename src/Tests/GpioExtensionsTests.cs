using System;
using System.Device.Gpio;
using TrafficLight.Client.Abstractions;
using TrafficLight.Client.Gpio.Extensions;
using Xunit;

namespace TrafficLight.Client.Tests
{
    public class GpioExtensionsTests
    {
        [Fact]
        public void ToGpioPinValueShouldReturnCorrectValue()
        {
            // Arrange
            var highValue = PinValue.High;
            var lowValue = PinValue.Low;

            // Act
            var gpioHighValue = highValue.ToGpioPinValue();
            var gpioLowValue = lowValue.ToGpioPinValue();

            // Assert
            Assert.Equal(GpioPinValue.High, gpioHighValue);
            Assert.Equal(GpioPinValue.Low, gpioLowValue);
        }

        [Fact]
        public void ToPinValueWithWrongValueShouldThrowException()
        {
            // Arrange
            var wrongValue = (GpioPinValue)42;

            // Act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => wrongValue.ToPinValue());
        }
    }
}