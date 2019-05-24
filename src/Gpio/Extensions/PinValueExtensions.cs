using System;
using System.Device.Gpio;
using TrafficLight.Client.Abstractions;

namespace TrafficLight.Client.Gpio.Extensions
{
    public static class PinValueExtensions
    {
        public static GpioPinValue ToGpioPinValue(this PinValue pinValue)
        {
            if (pinValue == PinValue.High)
            {
                return GpioPinValue.High;
            }

            if (pinValue == PinValue.Low)
            {
                return GpioPinValue.Low;
            }

            throw new ArgumentOutOfRangeException(nameof(pinValue));
        }

        public static PinValue ToPinValue(this GpioPinValue gpioPinValue)
        {
            switch (gpioPinValue)
            {
                case GpioPinValue.High:
                    return PinValue.High;
                case GpioPinValue.Low:
                    return PinValue.Low;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gpioPinValue));
            }
        }
    }
}