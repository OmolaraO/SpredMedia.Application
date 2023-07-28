using System;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.DTOs;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IProfileServices
	{
        Task<ResponseDto<IEnumerable<ProfileResponseDto>>> GetAllUserProfilesByIdAsync(string userId);
        Task<ResponseDto<CreateProfileDto>> CreateProfile(CreateProfileDto createProfileDto);
        Task<ResponseDto<ProfileResponseDto>> GetProfileByIdAsync(string id);
        Task<ResponseDto<ProfileResponseDto>> EditProfileAsync(string profileId, ProfileRequestDto requestDto);
        Task<ResponseDto<string>> DeleteProfile(string userId, string profileId);

    }
}

