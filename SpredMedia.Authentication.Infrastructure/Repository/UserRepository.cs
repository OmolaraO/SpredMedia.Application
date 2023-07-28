
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User, AuthenticationDbContext> , IUserRepository
    {
        public UserRepository(AuthenticationDbContext contexrt) : base(contexrt) 
        {
            
        }
    }
}
