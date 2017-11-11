using System;

namespace AiDollar.Edgar.Model
{
    public class Security
    {
        public object _id { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string Cusip { get; set; }

        public string SecurityDesc { get; set; }

        public static bool IsSameIsin(string isin1, string isin2)
        {
            var i1 = isin1.TrimStart(new[]{'0'});
            var i2 = isin2.TrimStart(new[] {'0'});

            return string.Compare(i1, i2, StringComparison.CurrentCultureIgnoreCase) == 0;
        }
       
    }
}
