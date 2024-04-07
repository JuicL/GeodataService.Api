using GeodataService.Api.DTOs;

namespace GeodataService.Api.Services.Contracts
{
    public interface IReverseGeocodingService
    {
        Task<IList<ResponseAddressDTO>> GetAddressesAsync(CoordinatesDTO coordinates);
    }
}