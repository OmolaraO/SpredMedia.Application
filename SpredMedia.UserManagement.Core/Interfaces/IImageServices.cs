using System;
using Microsoft.AspNetCore.Http;
using SpredMedia.CommonLibrary;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IImageServices
	{
        Task<ResponseDto<string>> UploadImageAsync(string Id, IFormFile file);
    }
}

