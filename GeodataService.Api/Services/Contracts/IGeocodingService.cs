using GeodataService.Api.DTOs;

namespace GeodataService.Api.Services.Contracts
{
    public interface IGeocodingService
    {
        Task<CoordinatesDTO?> GetCoordinatesAsync(RequestCoordinatesFromAddressDTO address);
    }
}