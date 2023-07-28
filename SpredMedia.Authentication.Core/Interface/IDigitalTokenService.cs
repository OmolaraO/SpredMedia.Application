using Microsoft.AspNetCore.Identity;
using SpredMedia.Authentication.Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface IDigitalTokenService
    {
        Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user);
        Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user);
    }
}
