using System.Collections.Generic;

namespace AiDollar.Edgar.Service.Service
{
    public class EdgarIdxDesc
    {
        public const string FormType = "Form Type";
        public const string CIK = "CIK";
        public const string DateFiled = "Date Filed";
        public const string URL = "URL";
        public const string CompanyName = "Company Name";

        public int StartFormType { get; set; }
        public int StartCIK { get; set; }
        public int StartDate { get; set; }
        public int StartURL { get; set; }
        public int StartCompanyName { get; set; }
        
        public IList<string> DataLines { get; set; }
        
    }
}
