using Microsoft.AspNetCore.Identity;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Model.model
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
