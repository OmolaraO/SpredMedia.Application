using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class ProfileRepository : GenericRepository<UserProfile>, IProfileRepository
    {
        private readonly UserManagementDbContext _dbContext;

        public ProfileRepository(UserManagementDbContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Gets profile by id from the database
        /// </summary>
        /// <param name="id">ID of profile</param>
        /// <returns>Returns profile if found and null if not found</returns>
        public async Task<UserProfile?> GetProfileById(string id)
        {
            return await _dbContext.UserProfiles.FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// Gets a list of all the profiles that belongs to a user
        /// </summary>
        /// <param name="">userId</param>
        /// <returns>Returns the list if found and null if not found</returns>
        public IQueryable<UserProfile> GetAllUserProfile(string userId)
        {
            return _dbContext.UserProfiles.Where(w => w.UserId == userId).Select(t => t);

        }

        /// <summary>
        /// Gets a list of all the profiles
        /// </summary>
        /// <param name=""></param>
        /// <returns>Returns the list if found and null if not found</returns>
        public async Task<List<UserProfile>> GetAllProfiles()
        {
            return await _dbContext.UserProfiles.ToListAsync();
        }
    }
}

