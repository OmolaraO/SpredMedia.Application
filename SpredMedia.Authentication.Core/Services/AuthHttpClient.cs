using Microsoft.AspNetCore.Http;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Services
{
    public class AuthHttpClient : ExternalClientRequest, IAuthClientService
    {
        public AuthHttpClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor):base(httpClientFactory, httpContextAccessor)
        {      
        }
    }
}
