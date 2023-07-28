using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;
using SpredMedia.UserManagement.Infrastructure;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class DownloadHistoryRepository : GenericRepository<DownloadHistory>, IDownloadHistoryRepository
    {
        private readonly UserManagementDbContext _dbContext;

        public DownloadHistoryRepository(UserManagementDbContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Gets download history by id from the database
        /// </summary>
        /// <param name="id">ID of download</param>
        /// <returns>Returns user if found and null if not found</returns>
        public async Task<DownloadHistory?> GetDownloadHistoryById(string id)
        {
            return await _dbContext.DownloadHistory.FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// Gets all the downloads that relate to a particular profile
        /// </summary>
        /// <param name=""></param>
        /// <returns>IQueryable of Download related to a given profile Id</returns>
        public IQueryable<DownloadHistory> GetAllDownloadHistory(string profileId)
        {
            return _dbContext.DownloadHistory.Where(w => w.ProfileId == profileId).Select(t => t);
        }
    }
}

