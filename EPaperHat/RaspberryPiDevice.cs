namespace EPaperHat
{
    public class RaspberryPiDevice : IDevice
    {
        public int RstPin => 17;
        public int DcPin => 25;
        public int CsPin => 8;
        public int BusyPin => 24;
        public int SpiFrequency => 4000000;
    }
}