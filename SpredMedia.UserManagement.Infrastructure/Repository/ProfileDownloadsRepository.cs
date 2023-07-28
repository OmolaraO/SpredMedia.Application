using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.Interface;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class ProfileDownloadsRepository : GenericRepository <ProfileDownloads,UserManagementDbContext>, IProfileDownloadRepoository
    {
        public ProfileDownloadsRepository(UserManagementDbContext context):base(context) 
        {
        }
    }
}
