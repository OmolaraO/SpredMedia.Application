using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface IClientAccountService
    {
        Task<ResponseDto<ClientResponseDto>> RegisterClient(ClientRequestDto ClientRequestDto);
        Task<ResponseDto<ClientResponseDto>> UpdateClient(ClientRequestDto ClientRequestDto);
        Task<ResponseDto<bool>> DeleteClient(string ClientId);
        Task<ResponseDto<ClientResponseDto>> GetClientByUser();
        Task<ResponseDto<PaginationResult<IEnumerable<ClientResponseDto>>>> GetAllClient(int pageNumber, int PageSize);
        Task<ResponseDto<bool>> AddClientEndpoint(string ClientId, string EndpointId);
        Task<ResponseDto<List<EndpointResponseDto>>> GetClientEndpoints(string ClientId);
        Task<ResponseDto<bool>> DeleteClientEndpoint(string ClientId, string EndpointId);
        Task<ResponseDto<KeyAndIv>> GetKeyAndIv(KeyAndIv keyAndIv);
    }
}
