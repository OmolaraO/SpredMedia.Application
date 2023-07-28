using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.DTOs;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpredMedia.UserManagement.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {

        private readonly IProfileServices _profileServices;
        public ProfileController(IProfileServices profileServices)
        {
            _profileServices = profileServices;
        }

        //Task<ResponseDto<IEnumerable<ProfileResponseDto>>> GetAllUserProfilesByIdAsync(string userId);
        //Task<ResponseDto<CreateProfileDto>> CreateProfile(User userModel, CreateProfileDto createProfileDto);
        //Task<ResponseDto<ProfileResponseDto>> GetProfileByIdAsync(string id);
        //Task<ResponseDto<ProfileResponseDto>> EditProfileAsync(string profileId, ProfileRequestDto requestDto);
        //Task<ResponseDto<string>> DeleteProfile(string userId, string profileId);

        /// <summary>
        /// Endpoint to get all profiles that belong to a user
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/get-all-user-profiles-by-userId")]
        public async Task<IActionResult> GetAllUserProfilesById(string userId)
        {
            var result = await _profileServices.GetAllUserProfilesByIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }



        /// <summary>
        /// Endpoint to get a single user profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{profileId}/get-a-user-profile-by-profileId")]
        public async Task<IActionResult> GetProfileById(string profileId)
        {
            var result = await _profileServices.GetProfileByIdAsync(profileId);
            return StatusCode(result.StatusCode, result);
        }



        /// <summary>
        /// Endpoint to create a user profile
        /// </summary>
        /// <param name="createProfileDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/create-a-profile")]
        public async Task<IActionResult> CreateProfile(CreateProfileDto createProfileDto)
        {
            var result = await _profileServices.CreateProfile(createProfileDto);
            return StatusCode(result.StatusCode, result);
        }



        /// <summary>
        /// Endpoint to edit a user profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{profileId}/edit-user-profile")]
        public async Task<IActionResult> EditProfile(string profileId, ProfileRequestDto requestDto)
        {
            var result = await _profileServices.EditProfileAsync(profileId, requestDto);
            return StatusCode(result.StatusCode, result);
        }



        /// <summary>
        /// Delete a user profile
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{profileId}/delete-profile")]
        public async Task<IActionResult> DeleteProfile(string userId, string profileId)
        {
            var result = await _profileServices.DeleteProfile(userId, profileId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
