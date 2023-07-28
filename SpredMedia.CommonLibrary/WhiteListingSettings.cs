using Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace SpredMedia.CommonLibrary
{
    public class WhiteListingSettings
    {
        private readonly RequestDelegate _next;
        private readonly string _allowedPort;
        private readonly string _allowedIP;
        public WhiteListingSettings(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _allowedIP = config.GetSection("AllowedIpAddress").GetValue<string>("IpConfig");
            _allowedPort = config.GetSection("AllowedIpAddress").GetValue<string>("PortConfig");
        }

        public async Task Invoke(HttpContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = ResponseDto<string>.Fail("Forbidden Access: Contact the Adminstrator");
            try
            {

                var remoteIpAddress = context.Connection.RemoteIpAddress.ToString();
                int remotePort = context.Connection.RemotePort;
                var localAddress = context.Connection.LocalIpAddress.ToString();
                int localPort = context.Connection.LocalPort;
                if (!IsIPAllowed(remoteIpAddress,remotePort, localPort, localAddress))
                {
                    // IP is not in the whitelist, return a 403 Forbidden response
                    response.StatusCode = StatusCodes.Status403Forbidden;
                    responseModel.Message = "Forbidden or Restricted";
                    await response.WriteAsJsonAsync(responseModel);
                    return;
                }

                // IP is allowed, proceed to the next middleware
                await _next(context);
            }
            catch (Exception)
            {
                // IP is not in the whitelist, return a 403 Forbidden response
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                responseModel.Message = "Forbidden or Restricted";
                await response.WriteAsJsonAsync(responseModel);
                return;
            }
        }

        // this endpoint performs the matching and returns the boolean to grant access
        private bool IsIPAllowed(string ipAddress, int port,int localPort, string localAddress)
        {
            return (ipAddress.ToString().Equals(_allowedIP) && port.ToString().Equals(_allowedPort)) ||
                   (localAddress.ToString().Equals(_allowedIP) && localPort.ToString().Equals(_allowedPort));
        }

    }
    public static class WhiteListingMiddleWare
    {
        public static IApplicationBuilder UseWhiteListingConfiguration(this IApplicationBuilder app, IConfiguration config)
        {
            return app.UseMiddleware<WhiteListingSettings>(config);
        }
    }
}
