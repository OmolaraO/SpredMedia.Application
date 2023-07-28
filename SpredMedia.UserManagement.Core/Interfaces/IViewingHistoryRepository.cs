using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Interfaces
{
    public interface IViewingHistoryRepository : IGenericRepository<ViewingHistory>
    {
        Task<ViewingHistory?> GetViewHistoryById(string id);
        IQueryable<ViewingHistory> GetAllViewHistory(string profileId);
    }
}