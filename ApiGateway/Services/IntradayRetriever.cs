using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Bam.Compliance.ApiGateway.Models;
using Newtonsoft.Json;

namespace Bam.Compliance.ApiGateway.Services
{
    public class IntradayRetriever : IIntradayRetriever
    {
        private const string TokenCall = "/oauth/token";
        private const string OrderCall = "/api/order/bysymbol?symbol=";
        private const string PositionCall = "/api/position/getDetailed?bamSymbol=";
        private readonly string _baseUrl;
        private static KeyValuePair<string, string>? _dateToken;
        public IntradayRetriever(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        private string Token
        {
            //no locking, cos the token will be the same for one single instance
            get
            {
                var todate = DateTime.Now.Date.ToString(CultureInfo.InvariantCulture);
                if (_dateToken == null || _dateToken.Value.Key != todate)
                {
                    _dateToken = new KeyValuePair<string, string>(todate, GetToken());
                }
                return _dateToken.Value.Value;
            }
        }

        public string GetToken()
        {
            var req = System.Net.WebRequest.Create($"{_baseUrl}{TokenCall}");
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.Credentials = CredentialCache.DefaultCredentials;

            //post data
            const string parameters = "grant_type=client_credentials&expiry=1.00%3A00%3A00";
            var bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            var os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();

            //get response
            var resp = req.GetResponse();
            var sr = new System.IO.StreamReader(resp.GetResponseStream());
            var json = sr.ReadToEnd().Trim();
            var token = JsonConvert.DeserializeObject<AccessToken>(json);

            return token.access_token;
        }

        public IEnumerable<Position> GetPositions(string bamSymbol)
        {
            var url = $"{_baseUrl}{PositionCall}{bamSymbol}";
            var json = GetJson(url);
            var positions = JsonConvert.DeserializeObject<IList<Position>>(json);

            return positions;
        }

        public IEnumerable<Trade> GetTrades(string bamSymbol)
        {
            var url = $"{_baseUrl}{OrderCall}{bamSymbol}";
            var json = GetJson(url);
                dynamic orders = JsonConvert.DeserializeObject(json);
                var trades = ((IEnumerable) orders).Cast<dynamic>()
                    .Where(o=>o.Status != "Deleted")
                    .Select(o => new AllocatedTrade()
                    {
                        BamSymbol=o.Security.BamSymbol,
                        Broker=o.Custodian,
                        Fund = "NA",
                        Quantity = o.Size,
                        Side = o.Side,
                        TradeDate = o.TradeDate,
                        Trader = o.Trader,
                        Strategy = o.Portfolio.PMCode,
                        TradeId = o.ClientOrderId
                     });

                return Util.Consolidate(trades);
        }

        private string GetJson(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + Token);

            // Get the Response
            using (var webResponse = request.GetResponse() as HttpWebResponse)
            {
                // Extract ResponseStream from Response
                var reader = new StreamReader(webResponse.GetResponseStream());
                var json = reader.ReadToEnd();
                return json;
            }
        }
    }
    
}
