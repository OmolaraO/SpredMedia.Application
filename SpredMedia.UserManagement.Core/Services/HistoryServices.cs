using System;
using System.Net;
using AutoMapper;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.DTOs;
using SpredMedia.UserManagement.Core.DTOs.HistoryDto;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Core.Utilities.Settings;
using SpredMedia.UserManagement.Model.Entity;
using ILogger = Serilog.ILogger;

namespace SpredMedia.UserManagement.Core.Services
{
	public class HistoryServices : IHistoryServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;
		private readonly ApplicationSettings _applicationSettings;

        //please you need to define a lifecyle for the is object 
        public HistoryServices(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper /*,ApplicationSettings applicationSettings*/)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
            //_applicationSettings = applicationSettings;
        }

		/// <summary>
		/// Gets all the downloads carried out in a particular profile and return it in pages
		/// </summary>
		/// <param name="profileId"></param>
		/// <param name="pageNumber"></param>
		/// <returns></returns>
		public async Task<ResponseDto<PaginationResult<IEnumerable<DownloadHistoryResponseDto>>>> GetDownloadHistoryAsync(string profileId, int pageNumber)
		{
            var profile = await _unitOfWork.UserProfile.GetProfileById(profileId);

            if (profile == null)
            {
                return ResponseDto<PaginationResult<IEnumerable<DownloadHistoryResponseDto>>>.Fail
                   ("UnSuccessfull", (int)HttpStatusCode.BadRequest);
            }

            var downloads = _unitOfWork.DownloadHistory.GetAllDownloadHistory(profileId).OrderByDescending(t => t.CreatedAt);

			var paginatedResult = await Paginator.PaginationAsync<DownloadHistory, DownloadHistoryResponseDto>
				(downloads, _applicationSettings.PageSize, pageNumber, _mapper);
			return ResponseDto<PaginationResult<IEnumerable<DownloadHistoryResponseDto>>>.Success("Successful", paginatedResult, (int)HttpStatusCode.OK);

		}


        /// <summary>
        /// Delete a single download history in the list
        /// </summary>
        /// <param name="downloadId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDownloadHistory(string downloadId, string profileId)
        {
            try
            {
                bool profile = await GetProfileByIdAsync(profileId);

                var downloads = _unitOfWork.DownloadHistory.GetAllDownloadHistory(profileId);

                var download = downloads.Where(x => x.Id == downloadId).FirstOrDefault();
                if (download == null)
                {
                    _logger.Information($"download with {downloadId} does not exist");
                    return ResponseDto<string>.Fail($"cannot delete {downloadId}, does not exist", (int)HttpStatusCode.BadRequest);
                }

                _unitOfWork.DownloadHistory.DeleteEntityAsync(download);
                await _unitOfWork.Save();
                _logger.Information($"movie with title {download.MovieTitle}has been deleted");

                return ResponseDto<string>.Success("Deleted", "Movie Download history successfully deleted", (int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Error($"Could not delete movie download history: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Gets all the views carried out in a particular profile and return it in pages
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PaginationResult<IEnumerable<ViewingHistoryResponseDto>>>> GetViewingHistoryAsync(string profileId, int pageNumber)
        {
            var profile = await _unitOfWork.UserProfile.GetProfileById(profileId);

            if (profile == null)
            {
                return ResponseDto<PaginationResult<IEnumerable<ViewingHistoryResponseDto>>>.Fail
                   ("UnSuccessfull", (int)HttpStatusCode.BadRequest);
            }

            var views = _unitOfWork.ViewingHistory.GetAllViewHistory(profileId).OrderByDescending(t => t.CreatedAt);

            var paginatedResult = await Paginator.PaginationAsync<ViewingHistory, ViewingHistoryResponseDto>
                (views, _applicationSettings.PageSize, pageNumber, _mapper);
            return ResponseDto<PaginationResult<IEnumerable<ViewingHistoryResponseDto>>>.Success("Successful", paginatedResult, (int)HttpStatusCode.OK);

        }

        /// <summary>
        /// Delete a single view history in the list
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteViewHistory(string viewId, string profileId)
        {
            try
            {
                bool profile = await GetProfileByIdAsync(profileId);

                var views = _unitOfWork.ViewingHistory.GetAllViewHistory(profileId);

                var view = views.Where(x => x.Id == viewId).FirstOrDefault();
                if (view == null)
                {
                    _logger.Information($"movie with {viewId} does not exist");
                    return ResponseDto<string>.Fail($"cannot delete {viewId}, does not exist", (int)HttpStatusCode.BadRequest);
                }

                _unitOfWork.ViewingHistory.DeleteEntityAsync(view);
                await _unitOfWork.Save();
                _logger.Information($"Moview with title {view.MovieTitle}has been deleted");

                return ResponseDto<string>.Success("Deleted", "Movie View history successfully deleted", (int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Error($"Could not delete view history: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> GetProfileByIdAsync(string id)
        {
            _logger.Information($"Attempting to fetch record for {id}");
            var profile = await _unitOfWork.UserProfile.GetProfileById(id);
            if (profile == null)
            {
                _logger.Information($"Profile with id = {id} does not exist in record");
                return false; ;
            }

            _logger.Information($"Profile with Id = {id}, exists");
            return true;
        }
    }
}

