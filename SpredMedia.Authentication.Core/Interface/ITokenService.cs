using SpredMedia.Authentication.Model.model;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        Task<string> GenerateToken(User user);
    }
}
