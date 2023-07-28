using System;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IUserRepository : IGenericRepository<User>
	{
        Task<User?> GetUserById(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsers();
    }
}

