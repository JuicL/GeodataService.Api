using Dadata;
using GeodataService.Api.DTOs;
using GeodataService.Api.Services.Contracts;

namespace GeodataService.Api.Services
{
    public class ReverseGeocodingService : IReverseGeocodingService
    {
        private readonly IConfiguration Configuration;

        public ReverseGeocodingService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IList<ResponseAddressDTO>> GetAddressesAsync(CoordinatesDTO coordinates)
        {
            var token = Configuration["API_KEY"];
            if (token is null)
                throw new Exception("Api key not found");

            var api = new SuggestClientAsync(token);
            var result = await api.Geolocate(lat: coordinates.Latitude, lon: coordinates.Longitude,count: 10, radius_meters: 100000);

            var addresses = new List<ResponseAddressDTO>();
            foreach (var suggestion in result.suggestions) 
            {
                addresses.Add(new ResponseAddressDTO { 
                    Region = suggestion.data.region,
                    City = suggestion.data.city,
                    Street = suggestion.data.street,
                    House = suggestion.data.house,
                    Flat = suggestion.data.flat
                });
            }
            return addresses;
        }
        
    }
}
