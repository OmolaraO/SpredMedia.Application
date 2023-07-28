using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpredMedia.UserManagement.Core.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpredMedia.UserManagement.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly IImageServices _imageServices;
        public ImageController(IImageServices imageServices)
        {
            _imageServices = imageServices;
        }

        /// <summary>
        /// Endpoint to upload a single omage on the user profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{profileId}/upload-image")]
        public async Task<IActionResult> UploadImage(string profileId, [FromForm] IFormFile file)
        {
            var result = await _imageServices.UploadImageAsync(profileId, file);
            return StatusCode(result.StatusCode, result);
        }
    }
}

