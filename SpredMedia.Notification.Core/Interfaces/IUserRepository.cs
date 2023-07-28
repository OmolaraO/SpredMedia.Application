using System;
using SpredMedia.Notification.Model.Entity;

namespace SpredMedia.Notification.Core.Interfaces
{
	public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserById(string id);
        Task<List<User>> GetAllUsers();
        Task<List<string>> GetAllUserAddresses();
    }
}

