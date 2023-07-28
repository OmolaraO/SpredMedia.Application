

using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface IUnitOfWork : IGenericUnitOfWork
    {
        IClientEndpointRepository ClientEndpointRepository { get; }
        IClientRepository ClientRepository { get; }
        IEndpointRepository EndpointRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
