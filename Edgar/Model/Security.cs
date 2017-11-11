﻿namespace AiDollar.Edgar.Model
{
    public class Security
    {
        public object _id { get; set; }
        public string Ticker { get; set; }
        public string SecurityDesc { get; set; }
        public string Isin { get; set; }

        public string GetCusip()
        {
            return Isin?.Length>11?Isin.Substring(2,9):null;
        }
    }
}