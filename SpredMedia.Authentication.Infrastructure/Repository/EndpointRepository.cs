using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Infrastructure.Repository
{
    public class EndpointRepository : GenericRepository<EndPoint, AuthenticationDbContext>, IEndpointRepository
    {
        public EndpointRepository(AuthenticationDbContext context) : base(context)
        {
            
        }
    }
}
