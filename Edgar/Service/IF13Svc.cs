namespace AiDollar.Edgar.Service.Service
{
    public interface IF13Svc
    {
        void GetLatest13F(string outputPath);
        void GetLatest13F(string outputPath, bool downloadNew);
    }
}
