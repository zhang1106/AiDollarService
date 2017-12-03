using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AiDollar.Edgar.Service.Model;

namespace AiDollar.Edgar.Service.Service
{
    public class EdgarIdxSvc : IEdgarIdxSvc
    {
        private readonly string _crawlerIdx;
       
        public EdgarIdxSvc(string crawlerIdx)
        {
            _crawlerIdx = crawlerIdx;
        }

        public IEnumerable<EdgarIdx> Parse(EdgarIdxDesc desc)
        {
            var idxes = new List<EdgarIdx>();
            foreach (var row in desc.DataLines)
            {
                var idx = new EdgarIdx
                {
                    CompanyName = row.Substring(desc.StartCompanyName, desc.StartFormType - desc.StartCompanyName)
                        .Trim(),
                    FormType = row.Substring(desc.StartFormType, desc.StartCIK - desc.StartFormType).Trim(),
                    CIK = row.Substring(desc.StartCIK, desc.StartDate - desc.StartCIK).Trim(),
                    DateFiled = DateTime.Parse(row.Substring(desc.StartDate, desc.StartURL - desc.StartDate)),
                    URL = row.Substring(desc.StartURL).Trim()
                };
                idxes.Add(idx);
            }
            return idxes;
        }

        public EdgarIdxDesc DownLoadEdgarIdx()
        {
            using (var client = new WebClient())
            {
                var idx = client.DownloadData(_crawlerIdx);
                var str = Encoding.UTF8.GetString(idx);
                var reader = new StringReader(str);
                var list = new List<string>();

                var idxDesc = new EdgarIdxDesc {DataLines = list};

                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null) break;
                    if (line.StartsWith("Company Name"))
                    {
                        idxDesc.StartCompanyName = line.IndexOf(EdgarIdxDesc.CompanyName);
                        idxDesc.StartFormType = line.IndexOf(EdgarIdxDesc.FormType);
                        idxDesc.StartCIK = line.IndexOf(EdgarIdxDesc.CIK);
                        idxDesc.StartDate = line.IndexOf(EdgarIdxDesc.DateFiled);
                        idxDesc.StartURL = line.IndexOf(EdgarIdxDesc.URL);
                    }

                    if (!line.StartsWith("-----")) continue;

                    while (true)
                    {
                        var dataline = reader.ReadLine();
                        if (dataline == null) break;
                        list.Add(dataline);
                    }
                    break;
                }

                return idxDesc;
            }
        }
    }
}