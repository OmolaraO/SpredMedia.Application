using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Model.Entity;

namespace SpredMedia.Notification.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly NotificationDbContext _dbContext;

        public UserRepository(NotificationDbContext context) : base(context)
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
        /// Gets a list of all the users
        /// </summary>
        /// <param name=""></param>
        /// <returns>Returns the list if found and null if not found</returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync(); 
        }

        /// <summary>
        /// Gets a list of all user's email addresses
        /// </summary>
        /// <returns>Retunrs the list, if found and null if not</returns>
        public async Task<List<string>> GetAllUserAddresses()
        {
            List<string> allAddresses = new List<string>();
            var allUsers = await GetAllUsers();

            foreach (User user in allUsers)
            {
                var address = user.EmailAddress;
                allAddresses.Add(address);
            }

            return allAddresses;
        }

    }

}

