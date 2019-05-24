using System;

namespace TrafficLight.Client.Abstractions
{
    public interface IGpioClient
    {
        void OpenOutputPin(int pinNumber);
        void OpenInputPin(int pinNumber, Action method);
        void WritePin(int pinNumber, GpioPinValue value);
    }
}