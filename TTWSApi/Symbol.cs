using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TTWSApi
{
    public class Symbol
    {
        private readonly HttpClient _client;
        private readonly TTWSSettings _ttws;
        private readonly AsyncRetryPolicy _policy;

        public Symbol(HttpClient client,TTWSSettings ttws)
        {
            _policy = Policy.Handle<Exception>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromSeconds(1)
            }, (exception, timespan) =>
            {
                Console.WriteLine(exception.Message,timespan);
            });
            _client = client;
            _ttws = ttws;
        }

        public async Task<string> GetByIsin(string isin)
        {
            HttpResponseMessage response = null;
            await _policy.ExecuteAsync(async () =>
            {
                response = await _client.GetAsync($"?action=getSymbolsByISIN&customerID={_ttws.CustomerId}&isin={isin}");
                response.EnsureSuccessStatusCode();
            });
        

            return await response.Content.ReadAsStringAsync();
        }

    }
}
