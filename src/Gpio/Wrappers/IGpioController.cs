using System.Device.Gpio;

namespace TrafficLight.Client.Gpio.Wrappers
{
    public interface IGpioController
    {
         void OpenPin(int pinNumber, PinMode mode);
         void Write(int pinNumber, PinValue value);
         void RegisterCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventType, PinChangeEventHandler callback);
    }
}