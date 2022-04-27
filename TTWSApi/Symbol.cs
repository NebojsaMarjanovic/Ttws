using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TTWSApi
{
    public class Symbol
    {
        private readonly HttpClient _client;
        private readonly TTWSSettings _ttws;

        public Symbol(HttpClient client,TTWSSettings ttws)
        {
            _client = client;
            _ttws = ttws;
        }

        public async Task<string> GetByIsin(string isin)
        {

            var response = await _client.GetAsync($"?action=getSymbolsByISIN&customerID={_ttws.CustomerId}&isin={isin}"); 
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
