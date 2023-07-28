using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManagementDbContext _dbContext;

        public UserRepository(UserManagementDbContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Gets user by id from the database
        /// </summary>
        /// <param name="id">ID of user</param>
        /// <returns>Returns user if found and null if not found</returns>
        public async Task<User?> GetUserById(string id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// Gets user by email address from the database
        /// </summary>
        /// <param name="id">Email Address of user</param>
        /// <returns>Returns user if found and null if not found</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(w => w.EmailAddress == email);
        }

        /// <summary>
        /// Gets a list of all the users
        /// </summary>
        /// <param name=""></param>
        /// <returns>Returns the list if found and null if not found</returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}