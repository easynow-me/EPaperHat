namespace EPaperHat
{
    public interface IEPaper
    {
        void Initialize(int width, int height);
        void Clear();
        void Sleep();
        void ShowImage(byte[] image);
    }
}
