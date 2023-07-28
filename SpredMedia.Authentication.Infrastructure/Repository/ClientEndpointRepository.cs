using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;


namespace SpredMedia.Authentication.Infrastructure.Repository
{
    public class ClientEndpointRepository : GenericRepository <ClientEndpoint,AuthenticationDbContext>, IClientEndpointRepository
    {
        public ClientEndpointRepository(AuthenticationDbContext context) : base(context) 
        {

        }
    }
}
