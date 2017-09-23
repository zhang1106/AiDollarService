using System.Net.Http;
using System.Threading.Tasks;

namespace AiDollar.Edgar.Service
{
    public class HttpDataAgent : IHttpDataAgent
    {
        public async Task<byte[]> DownloadPageAsync(string page)
        {
            // ... Use HttpClient.
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(page))
            using (var content = response.Content)
            {
                // ... Read the string.
                var result = await content.ReadAsByteArrayAsync();

                return result;

            }
        }


    }
}
