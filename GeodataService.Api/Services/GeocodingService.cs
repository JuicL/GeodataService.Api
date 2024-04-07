using GeodataService.Api.DTOs;
using GeodataService.Api.Services.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace GeodataService.Api.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeocodingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
        }

        public async Task<CoordinatesDTO?> GetCoordinatesAsync(RequestCoordinatesFromAddressDTO address)
        {
            var queryBuilder = new QueryBuilder
            {
                { "сity", address.City },
                { "сounty", address.County },
                { "street", address.Street },
                { "format", "json" },
                { "limit", "2" },
            };
           
            var uriBuilder = new UriBuilder(_configuration["GeocodingService"]);
            uriBuilder.Query = queryBuilder.ToQueryString().Value;
            uriBuilder.Port = -1;
            
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(uriBuilder.Uri);
            var content = await httpResponse.Content.ReadAsStringAsync();
            if(!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            httpResponse.EnsureSuccessStatusCode();
            var coordinates = JsonConvert.DeserializeObject<List<CoordinatesDTO>>(content);
            return coordinates?.FirstOrDefault();
        }
    }
}
