using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using TrafficLight.Client.Abstractions;
using TrafficLight.Client.Gpio.Extensions;
using TrafficLight.Client.Gpio.Wrappers;

namespace TrafficLight.Client.Gpio
{
    public class LinuxGpioClient : IGpioClient
    {
        private Wrappers.IGpioController _controller;

        public LinuxGpioClient(Wrappers.IGpioController controller)
        {
            _controller = controller;
        }

        public void OpenOutputPin(int pinNumber)
        {
            _controller.OpenPin(pinNumber, PinMode.Output);
        }

        public void OpenInputPin(int pinNumber, Action method)
        {
            _controller.OpenPin(pinNumber, PinMode.InputPullUp);

            _controller.RegisterCallbackForPinValueChangedEvent(
                pinNumber, 
                PinEventTypes.Rising, 
                (sender, args) => method());
        }

        public void WritePin(int pinNumber, GpioPinValue value)
        {
            _controller.Write(pinNumber, value.ToPinValue());
        }
    }
}
