using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace TrafficLight.Client.Gpio.Wrappers
{
    public class RpiGpioController : IGpioController
    {
        private GpioController _real;

        public RpiGpioController()
        {
            _real = new GpioController(PinNumberingScheme.Logical, new RaspberryPi3Driver());
        }

        public void OpenPin(int pinNumber, PinMode mode)
        {
            _real.OpenPin(pinNumber, mode);
        }

        public void RegisterCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventType, PinChangeEventHandler callback)
        {
            _real.RegisterCallbackForPinValueChangedEvent(pinNumber, eventType, callback);
        }

        public void Write(int pinNumber, PinValue value)
        {
            _real.Write(pinNumber, value);
        }
    }
}