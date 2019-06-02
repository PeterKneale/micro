using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Micro.Services.Web.Services
{
    public interface IGatewayApi
    {
        Task<T> ExecuteGraphQuery<T>(string query);
    }

    public class GatewayApi : IGatewayApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<GatewayApi> _log;

        public GatewayApi(HttpClient client, ILogger<GatewayApi> log)
        {
            _client = client;
            _log = log;
        }

        public async Task<T> ExecuteGraphQuery<T>(string query)
        {
            var url = $"http://localhost:5000/graphql?query={query}";
            var response = await _client.GetAsync(url);
            var status = response.StatusCode;
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _log.LogError("Api {url} returned {status} {content}", url, status, content);
                response.EnsureSuccessStatusCode();
            }

            _log.LogInformation("Api {url} returned {status} {content}", url, status, content);
            Console.WriteLine($"Api {url} returned {status} {content}");
            return JsonConvert.DeserializeObject<GraphQueryResponse<T>>(await response.Content.ReadAsStringAsync()).Data;
        }
    }
    public class GraphQueryResponse<T>
    {
        public T Data { get; set; }
    }
}