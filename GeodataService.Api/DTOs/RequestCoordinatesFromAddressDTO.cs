namespace GeodataService.Api.DTOs
{
    public class RequestCoordinatesFromAddressDTO
    {
        public string County { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
