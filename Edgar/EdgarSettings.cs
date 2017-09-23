using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AiDollar.Infrastructure.Configuration;

namespace AiDollar.Edgar.Service
{
    public class EdgarSettings: SettingsBase
    {
        public string[] Ciks {get { return GetValue(() => Ciks); }}
        public string EdgarArchiveRoot { get { return GetValue(() => EdgarArchiveRoot); } }
        public string DataPath { get { return GetValue(() => DataPath); } }
    }
}
