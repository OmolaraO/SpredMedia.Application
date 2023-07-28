using System;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.DTOs.HistoryDto;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IHistoryServices
	{
        Task<ResponseDto<PaginationResult<IEnumerable<DownloadHistoryResponseDto>>>> GetDownloadHistoryAsync(string profileId, int pageNumber);
        Task<ResponseDto<string>> DeleteDownloadHistory(string downloadId, string profileId);
        Task<ResponseDto<PaginationResult<IEnumerable<ViewingHistoryResponseDto>>>> GetViewingHistoryAsync(string profileId, int pageNumber);
        Task<ResponseDto<string>> DeleteViewHistory(string viewId, string profileId);
        Task<bool> GetProfileByIdAsync(string id);
    }
}

