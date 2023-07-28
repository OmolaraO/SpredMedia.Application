using SpredMedia.Authentication.Core.Interface;
using SpredMedia.CommonLibrary;


namespace SpredMedia.Authentication.Infrastructure.Repository
{

    public class UnitOfWork : GenericUnitOfWork<AuthenticationDbContext>, IUnitOfWork
    {
        private IUserRepository _userRepository;
        private IEndpointRepository _endpointRepository;
        private IClientRepository _clientRepository;
        private IClientEndpointRepository _clientEndpointRepository;
        public UnitOfWork(AuthenticationDbContext context) : base(context)
        {
        }

        // add the model/ table to  instantiated here

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public IEndpointRepository EndpointRepository => _endpointRepository ??= new EndpointRepository(_context);
        public IClientRepository ClientRepository => _clientRepository ??= new ClientRepository(_context);
        public IClientEndpointRepository ClientEndpointRepository => _clientEndpointRepository ??= new ClientEndpointRepository(_context);

    }
}
