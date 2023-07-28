

using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Infrastructure.Repository
{
    public class ClientRepository : GenericRepository<Client,AuthenticationDbContext>, IClientRepository
    {
        public ClientRepository(AuthenticationDbContext context) : base(context) 
        {
            
        }
    }
}
