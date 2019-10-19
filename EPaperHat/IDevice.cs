namespace EPaperHat
{
    public interface IDevice
    {
        int RstPin { get; }
        int DcPin { get; }
        int CsPin { get; }
        int BusyPin { get; }
        int SpiFrequency { get; }
    }
}