
namespace SpredMedia.Authentication.Core.DTO
{
    public class RefreshTokenResponseDTO
    {
        public string NewAccessToken { get; set; }
        public string NewRefreshToken { get; set; }
    }
}