using SpredMedia.Authentication.Core.DTO;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Interface
{
    public interface IEndpointServices
    {
        Task<ResponseDto<EndpointResponseDto>> RegisterEndpoint(EndpointRequestDto endpointRequestDto);
        Task<ResponseDto<EndpointResponseDto>> UpdateEndpoint(EndpointRequestDto endpointRequestDto);
        Task<ResponseDto<bool>> DeleteEndpoint(string EndpointId);
        Task<ResponseDto<List<ClientResponseDto>>> GetClients(string EndpointId);
        Task<ResponseDto<PaginationResult<IEnumerable<EndpointResponseDto>>>> GetAllEndpoints(int pageSize, int pageNumber);
    }
}
