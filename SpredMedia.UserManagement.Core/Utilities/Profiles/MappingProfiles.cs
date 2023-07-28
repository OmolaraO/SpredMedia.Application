using System;
using SpredMedia.UserManagement.Core.DTOs;
using SpredMedia.UserManagement.Core.DTOs.HistoryDto;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Utilities.Profiles
{
	public class MappingProfiles : AutoMapper.Profile
    {
		public MappingProfiles()
		{
            CreateMap<DownloadHistoryResponseDto, DownloadHistory>().ReverseMap();
            CreateMap<ViewingHistoryResponseDto, ViewingHistory>().ReverseMap();
            CreateMap<ProfileResponseDto, UserProfile>().ReverseMap();
            CreateMap<ProfileRequestDto, UserProfile>();
            CreateMap<Profile, CreateProfileDto>();
        }
	}
}

