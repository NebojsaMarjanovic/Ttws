using System.Net.Http;
using System.Threading.Tasks;

namespace TTWSApi
{
    public class Symbol
    {
        private readonly HttpClient _client;

        public Symbol(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetByIsin(string isin)
        {
            var response = await _client.GetAsync("?action=getSymbolsByISIN&customerID=10&isin=" + isin); //izmeniti
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
