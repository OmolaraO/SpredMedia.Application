using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.Interfaces;
using System.Net;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Services
{
	public class ImageServices : IImageServices
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryServices _cloudinaryServices;

        public ImageServices(IUnitOfWork unitOfWork, ICloudinaryServices cloudinaryServices)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryServices = cloudinaryServices;
        }

        public async Task<ResponseDto<string>> UploadImageAsync(string id, IFormFile file)
        {


            Log.Information("Successfull enter the image upload service");
            var userprofile = await _unitOfWork.UserProfile.GetProfileById(id);
            var response = new ResponseDto<string>();
            if (userprofile == null)
            {
                Log.Information("User is not found");
                return ResponseDto<string>.Fail("Image upload failed", (int)HttpStatusCode.BadGateway);
            }
            Log.Information("User is found");
            var upload = await _cloudinaryServices.UploadImage(file);
            Log.Information("Successful upload the image");
            userprofile.ImageUrl = upload.Url.ToString();
            userprofile.ImagePublicId = upload.PublicId;

             _unitOfWork.UserProfile.Update(userprofile);
            await _unitOfWork.Save();
            return ResponseDto<string>.Success("Image uploaded successfully", userprofile.ImageUrl, (int)HttpStatusCode.OK);
        }
    }
}

