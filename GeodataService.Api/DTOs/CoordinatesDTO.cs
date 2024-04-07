using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GeodataService.Api.DTOs
{
    public class CoordinatesDTO
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}