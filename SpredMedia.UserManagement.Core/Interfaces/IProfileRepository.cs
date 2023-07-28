using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IProfileRepository : IGenericRepository<UserProfile>
	{
        Task<UserProfile?> GetProfileById(string id);
        IQueryable<UserProfile> GetAllUserProfile(string userId);
        Task<List<UserProfile>> GetAllProfiles();
    }
}

