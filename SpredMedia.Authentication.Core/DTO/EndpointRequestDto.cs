
namespace SpredMedia.Authentication.Core.DTO
{
    public class EndpointRequestDto
    {
        public string EndpointId { get; set; }
        public string ControllerName { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string Channel { get; set; }
    }
}
