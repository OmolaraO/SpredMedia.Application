using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.DTOs;
using SpredMedia.UserManagement.Core.DTOs.HistoryDto;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Core.Utilities.Settings;
using SpredMedia.UserManagement.Model.Entity;
using static SpredMedia.CommonLibrary.ExternalClientRequest;
using ILogger = Serilog.ILogger;


namespace SpredMedia.UserManagement.Core.Services
{
	public class ProfileServices : IProfileServices
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProfileServices(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        ///// <summary>
        ///// Returns all profiles that belongs to a particular user
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        public async Task<ResponseDto<IEnumerable<ProfileResponseDto>>> GetAllUserProfilesByIdAsync(string userId)
        {
            _logger.Information($"Attempting to fetch all user profiles for {userId}");
            var profiles =  _unitOfWork.UserProfile.GetAllUserProfile(userId);

            if (profiles == null)
            {
                return ResponseDto<IEnumerable<ProfileResponseDto>>.Fail
                   ("UnSuccessfull", (int)HttpStatusCode.BadRequest);
            }

            var profileList = _mapper.Map<IEnumerable<ProfileResponseDto>>(profiles);
            return ResponseDto<IEnumerable<ProfileResponseDto>>.Success("Successful", profileList, 200);
        }


        /// <summary>
        /// Create a user Profile
        /// </summary>
        /// <param name="createProfileDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto<CreateProfileDto>> CreateProfile(CreateProfileDto createProfileDto)
        {
            UserProfile userProfile;

            try
            {
                var getUser = await _unitOfWork.User.GetUserByEmailAsync(createProfileDto.Address);
                if (getUser == null)
                {
                    _logger.Information($"Could not user ");
                    return ResponseDto<CreateProfileDto>
                        .Fail($"Could not get user: {createProfileDto.Address}", (int)HttpStatusCode.BadRequest);
                }

                userProfile = _mapper.Map<UserProfile>(createProfileDto);
                userProfile.Username = getUser.FirstName + getUser.LastName;
                userProfile.Address = getUser.EmailAddress;
                userProfile.CreationDate = DateTimeOffset.Now;
                userProfile.UserId = getUser.Id;

                await _unitOfWork.UserProfile.InsertAsync(userProfile);
                await _unitOfWork.Save();
                _logger.Information($"Successfully added profile {userProfile.Id} to the database.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Could not create profile: {ex.Message}");
                return ResponseDto<CreateProfileDto>.Fail($"Could not create profile: {ex.Message}", (int)HttpStatusCode.BadRequest);
            }

            var profileResponse = _mapper.Map<CreateProfileDto>(userProfile);
            return ResponseDto<CreateProfileDto>.Success("Profile successfully created", profileResponse, (int)HttpStatusCode.Created);
        }



        /// <summary>
        /// Returns a particular user profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<ProfileResponseDto>> GetProfileByIdAsync(string id)
        {
            _logger.Information($"Attempting to fetch record for {id}");
            var profile = await _unitOfWork.UserProfile.GetProfileById(id);
            if (profile == null)
            {
                _logger.Information($"Profile with id = {id} does not exist in record");
                return ResponseDto<ProfileResponseDto>.Fail($"Profile with id = {id} does not exist in record");
            }

            _logger.Information($"Profile with Id = {id}, retrieved successfully");
            var profileDto = _mapper.Map<ProfileResponseDto>(profile);
            return ResponseDto<ProfileResponseDto>.Success("Profile successfully retrieved", profileDto);
        }




        /// <summary>
        /// Create a user Profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto<ProfileResponseDto>> EditProfileAsync(string profileId, ProfileRequestDto requestDto)
        {
            _logger.Information($"Attempting to fetch record for {profileId}");
            var profile = await _unitOfWork.UserProfile.GetProfileById(profileId);
            if (profile == null)
            {
                _logger.Information($"Profile with id = {profileId} does not exist in record");
                return ResponseDto<ProfileResponseDto>.Fail($"Profile with id = {profileId} does not exist in record");
            }

            _logger.Information($"Profile with Id = {profileId}, retrieved successfully");


            try
            {
                profile.Username = requestDto.Username;
                profile.DateOfBirth = requestDto.DateOfBirth;
                profile.Gender = requestDto.Gender;
                profile.PhoneNumber = requestDto.PhoneNumber;
                profile.ImageUrl = requestDto.ImageUrl;
                profile.ImagePublicId = requestDto.ImagePublicId;
                profile.UpdatedDateTime = DateTimeOffset.Now;
                profile.UserId = requestDto.UserId;

                _unitOfWork.UserProfile.Update(profile);
                await _unitOfWork.Save();
                _logger.Information($"Successfully updated profile {profile.Id} to the database.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Could not update profile: {ex.Message}");
                return ResponseDto<ProfileResponseDto>.Fail($"Could not create profile: {ex.Message}", (int)HttpStatusCode.BadRequest);
            }

            var profileDto = _mapper.Map<ProfileResponseDto>(profile);
            return ResponseDto<ProfileResponseDto>.Success("Profile successfully created", profileDto, (int)HttpStatusCode.Created);
        }



        /// <summary>
        /// Delete a single profile in the list
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteProfile(string userId, string profileId)
        {
            try
            {
                var user = await _unitOfWork.User.GetUserById(userId);

                if (user == null)
                {
                    _logger.Information("user is null");
                    return ResponseDto<string>.Fail("invalid userId", (int)HttpStatusCode.BadRequest);
                }

                var profiles = _unitOfWork.UserProfile.GetAllUserProfile(userId);
                var profile = profiles.Where(x => x.Id == profileId).FirstOrDefault();
                if (profile == null)
                {
                    _logger.Information($"User profile with {profileId} does not exist");
                    return ResponseDto<string>.Fail($"cannot delete {profileId}, does not exist", (int)HttpStatusCode.BadRequest);
                }

                _unitOfWork.UserProfile.DeleteEntityAsync(profile);
                await _unitOfWork.Save();
                _logger.Information($"User profile {profile.Id} has been deleted");

                return ResponseDto<string>.Success("Deleted", "User Profile successfully deleted", (int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Error($"Could not delete profile: {ex.Message}");
                throw;
            }
        }

    }
}

