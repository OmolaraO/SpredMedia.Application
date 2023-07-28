using System;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IDownloadHistoryRepository : IGenericRepository<DownloadHistory>
	{
        Task<DownloadHistory?> GetDownloadHistoryById(string id);
        IQueryable<DownloadHistory> GetAllDownloadHistory(string profileId);

    }
}

