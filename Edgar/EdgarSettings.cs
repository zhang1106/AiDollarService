using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiDollar.Infrastructure.Configuration;

namespace AiDollar.Edgar.Service
{
    public class EdgarSettings: AppSettingsBase
    {
        public string[] Ciks {get { return GetValue(() => Ciks); }}
        public string EdgarArchiveRoot { get { return GetValue(() => EdgarArchiveRoot); } }
    }
}
