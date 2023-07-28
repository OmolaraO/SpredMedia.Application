
namespace SpredMedia.Authentication.Core.DTO
{
    public class ClientRequestDto
    {
        public string ClientPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ClientUsername { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
    }
}
