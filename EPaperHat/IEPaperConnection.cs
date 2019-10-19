using Unosquare.RaspberryIO.Abstractions;

namespace EPaperHat
{
    public interface IEPaperConnection
    {
        IGpioPin RstPin { get; }
        IGpioPin DcPin { get; }
        IGpioPin CsPin { get; }
        IGpioPin BusyPin { get; }
        ISpiChannel Channel { get; }

        void SendCommand(byte command);
        void SendData(byte data);
    }
}