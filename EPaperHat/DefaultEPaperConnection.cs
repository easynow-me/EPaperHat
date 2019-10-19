using System;
using System.Runtime.CompilerServices;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO;

namespace EPaperHat
{
    public class DefaultEPaperConnection : IEPaperConnection
    {
        protected static readonly object LockObj = new object();

        public IGpioPin RstPin { get; }
        public IGpioPin DcPin { get; }
        public IGpioPin CsPin { get; }
        public IGpioPin BusyPin { get; }
        public ISpiChannel Channel { get; }

    public DefaultEPaperConnection(IDevice device)
        {
            RstPin = Pi.Gpio[device.RstPin];
            RstPin.PinMode = GpioPinDriveMode.Output;

            DcPin = Pi.Gpio[device.DcPin];
            DcPin.PinMode = GpioPinDriveMode.Output;

            CsPin = Pi.Gpio[device.CsPin];
            CsPin.PinMode = GpioPinDriveMode.Output;

            BusyPin = Pi.Gpio[device.BusyPin];
            BusyPin.PinMode = GpioPinDriveMode.Input;

            Pi.Spi.Channel0Frequency = device.SpiFrequency;
            Channel = Pi.Spi.Channel0;
        }

        public virtual void SendCommand(byte command)
        {
            lock (LockObj)
            {
                this.DcPin.Write(GpioPinValue.Low);
                this.CsPin.Write(GpioPinValue.Low);
                this.Channel.SendReceive(new[] {command});
                this.CsPin.Write(GpioPinValue.High);
            }
        }

        public virtual void SendData(byte data)
        {
            lock (LockObj)
            {
                this.DcPin.Write(GpioPinValue.High);
                this.CsPin.Write(GpioPinValue.Low);
                this.Channel.SendReceive(new[] { data });
                this.CsPin.Write(GpioPinValue.High);
            }
        }
    }
}