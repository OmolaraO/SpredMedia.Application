using System;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface ICloudinaryServices
	{
        Task<UploadResult> UpdateByPublicId(IFormFile file, string publicId);
        Task<UploadResult> UploadImage(IFormFile file);
    }
}

