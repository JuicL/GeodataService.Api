using GeodataService.Api.DTOs;
using GeodataService.Api.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GeodataService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GeolocateController : Controller
    {
        private readonly IGeocodingService _geocodingService;
        private readonly IReverseGeocodingService _reverseGeocodingService;
        private readonly ILogger<GeolocateController> _logger;

        public GeolocateController(ILogger<GeolocateController> logger,IGeocodingService geocodingService, IReverseGeocodingService reverseGeocodingService)
        {
            _logger = logger ?? throw new NullReferenceException(nameof(logger));
            _geocodingService = geocodingService ?? throw new NullReferenceException(nameof(geocodingService));
            _reverseGeocodingService = reverseGeocodingService ?? throw new NullReferenceException(nameof(reverseGeocodingService));
        }

        //Адресс по координатам
        [HttpGet]
        [Route("addresses")]
        [ProducesResponseType<IList<ResponseAddressDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Address([FromQuery] RequestAddressFromCoordinatesDTO requestAddressDTO) 
        {
            try
            {
                var addresses = await _reverseGeocodingService.GetAddressesAsync(new CoordinatesDTO()
                {
                    Latitude = requestAddressDTO.Latitude,
                    Longitude = requestAddressDTO.Longitude
                });
                _logger.LogInformation("Request for addresses with coordinates (lan:{0},lot:{1}) finished with status: {2}. Result: {3}",
                    requestAddressDTO.Latitude,
                    requestAddressDTO.Longitude,
                    addresses.Count == 0 ? "successfully" : "not found",
                    JsonSerializer.Serialize(addresses));

                if (addresses.Count == 0)
                {
                    return NotFound("No addresses found for this coordinates");
                }
                return Ok(addresses);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Request addresses for coordinates (lan:{0},lot:{1}) failed!",
                    requestAddressDTO.Latitude,
                    requestAddressDTO.Longitude
                    );
                return BadRequest(ex.Message);
            }
            
        }
        //Координаты по адресу
        [HttpGet]
        [Route("coordinates")]
        [ProducesResponseType<CoordinatesDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CoordinatesAsync([FromQuery] RequestCoordinatesFromAddressDTO requestCoordinatesDTO)
        {
            try
            {
                var coordinates = await _geocodingService.GetCoordinatesAsync(requestCoordinatesDTO);

                _logger.LogInformation("Request coordinates for address (County:{0},City:{1},Street:{2}) finished with status: {3}. Result: {4}",
                 requestCoordinatesDTO.County,
                  requestCoordinatesDTO.City,
                  requestCoordinatesDTO.Street,
                  coordinates is not null ? "successfully" : "not found",
                 JsonSerializer.Serialize(coordinates));

                if(coordinates is null) 
                {
                    return NotFound("No coordinates found for this address");            
                }
                return Ok(coordinates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request coordinates for address (County:{0},City:{1},Street:{2}) failed!",
                    requestCoordinatesDTO.County,
                  requestCoordinatesDTO.City,
                  requestCoordinatesDTO.Street);

                return BadRequest(ex.Message);
            }
        }
    }
}
