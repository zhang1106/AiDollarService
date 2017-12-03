using AiDollar.Infrastructure.Configuration;

namespace AiDollar.Edgar.Service
{
    public class EdgarSettings: SettingsBase
    {
        public string[] Ciks {get { return GetValue(() => Ciks); }}
        public string EdgarArchiveRoot { get { return GetValue(() => EdgarArchiveRoot); } }
        public string DataPath { get { return GetValue(() => DataPath); } }
        public string PosPage { get { return GetValue(() => PosPage); } }
        public string AiDollarMongo { get { return GetValue(() => AiDollarMongo); } }
        public string AiDollarDb { get { return GetValue(() => AiDollarDb); } }
        public string EdgarCrawlerUri { get { return GetValue(() => EdgarCrawlerUri); } }
        public int NumHeaderlines { get { return GetValue(() => NumHeaderlines); } }
        public int F4InDays {
            get { return GetValue(() => F4InDays); }
        }

    }
}
