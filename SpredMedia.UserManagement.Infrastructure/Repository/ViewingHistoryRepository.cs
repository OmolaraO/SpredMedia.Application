using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;
using SpredMedia.UserManagement.Infrastructure;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class ViewingHistoryRepository : GenericRepository<ViewingHistory>, IViewingHistoryRepository
    {
        private readonly UserManagementDbContext _dbContext;

        public ViewingHistoryRepository(UserManagementDbContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Gets view history by id from the database
        /// </summary>
        /// <param name="id">ID of viewed movie</param>
        /// <returns>Returns user if found and null if not found</returns>
        public async Task<ViewingHistory?> GetViewHistoryById(string id)
        {
            return await _dbContext.ViewingHistories.FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// Gets all the views that relate to a particular profile
        /// </summary>
        /// <param name=""></param>
        /// <returns>IQueryable of Views related to a given profile Id</returns>
        public IQueryable<ViewingHistory> GetAllViewHistory(string profileId)
        {
            return _dbContext.ViewingHistories.Where(w => w.ProfileId == profileId).Select(t => t);
        }
    }
}

