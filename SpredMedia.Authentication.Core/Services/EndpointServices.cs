using AutoMapper;
using EasyEncryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Core.Utility;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Authentication.Core.Services
{
    public class EndpointServices : IEndpointServices
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EndpointServices(IServiceProvider service)
        {

            _mapper = service.GetRequiredService<IMapper>();
            _logger = service.GetRequiredService<ILogger>();
            _unitOfWork = service.GetRequiredService<IUnitOfWork>();
        }
        public async Task<ResponseDto<bool>> DeleteEndpoint(string EndpointId)
        {
            try
            {
                _logger.Information("about to remove the endpoint from the database");
                var Endpoint = await _unitOfWork.EndpointRepository.GetAsync(x => x.Id.Equals(EndpointId));
                if (Endpoint is not null)
                {
                    _logger.Information("the instance exist in the database");
                    _logger.Information("proceeding to delete instance from from the database");
                    await _unitOfWork.EndpointRepository.DeleteAsync(EndpointId);
                    await _unitOfWork.Save();
                    return ResponseDto<bool>.Success("successfully saved the new updates added to the database", true, (int)System.Net.HttpStatusCode.Accepted);
                }
                return ResponseDto<bool>.Fail("the client doesnt exit in the database", (int)System.Net.HttpStatusCode.BadGateway);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<PaginationResult<IEnumerable<EndpointResponseDto>>>> GetAllEndpoints(int pageSize, int pageNumber)
        {
            try
            {
                _logger.Information("getting all the Endpoint  from the Endpoint DB");
                var listOfEndpoint = _unitOfWork.EndpointRepository.GetAllAsync();
                _logger.Information("used the pagination to limit the traffic of data coming from the database");
                var ClientValue = await listOfEndpoint.PaginationAsync<EndPoint, EndpointResponseDto>(pageSize, pageNumber, _mapper);
                return ResponseDto<PaginationResult<IEnumerable<EndpointResponseDto>>>.Success("successfully gotten the all the endpoint in the database", ClientValue, (int)System.Net.HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<List<ClientResponseDto>>> GetClients(string EndpointId)
        {
            try
            {
                _logger.Information("get the Endpoint entity using the entity framework");
                EndPoint clientEntity = await _unitOfWork.EndpointRepository.GetAllAsync().Where(x => x.Id.Equals(EndpointId)).Include("Clients").FirstOrDefaultAsync();
                if (clientEntity is not null)
                {
                    _logger.Information("Getting the list of the model enpoint associated to the client Instances");
                    var listOfClients = clientEntity.Clients.ToList();
                    List<ClientResponseDto> listOfEndPointResponse = listOfClients.Select(x => _mapper.Map<ClientResponseDto>(x)).ToList();
                    _logger.Information($"{listOfClients.Count} endpoints count are listed for the user Endpoint application");
                    if (listOfEndPointResponse.Count > 0)
                    {
                        return ResponseDto<List<ClientResponseDto>>.Success("the endpoint Client are", listOfEndPointResponse, (int)System.Net.HttpStatusCode.Accepted);
                    }
                    return ResponseDto<List<ClientResponseDto>>.Success("they are not Client associated to the service Endpoint", listOfEndPointResponse, (int)System.Net.HttpStatusCode.OK);
                }
                return ResponseDto<List<ClientResponseDto>>.Fail("the Endpoint does not Exist in the database", (int)System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<EndpointResponseDto>> RegisterEndpoint(EndpointRequestDto endpointRequestDto)
        {
            try
            {
                _logger.Information("the endpoint is about to be created withe following model " + JsonConvert.SerializeObject(endpointRequestDto));
                // map the Endpoint dto to the Endpoint Request model
                _logger.Information("mapping Dto to the object model");
                EndPoint EndPointObject = _mapper.Map<EndPoint>(endpointRequestDto);
                await _unitOfWork.EndpointRepository.InsertAsync(EndPointObject);
                _logger.Information("the instance have been added to the database");
                // get the instance from the session just to make sure that the instance has saved
                await _unitOfWork.Save();
                EndPoint EndpointResp = await _unitOfWork.EndpointRepository.GetAsync(x => x.Id.Equals(EndPointObject.Id));
                if (EndpointResp != null)
                {
                    _logger.Information("successfully saved to the database ");
                    // mapping the client details to the 
                    EndpointResponseDto Resp = _mapper.Map<EndpointResponseDto>(EndpointResp);
                    return ResponseDto<EndpointResponseDto>.Success("the endpoint has been created successful", Resp, (int)System.Net.HttpStatusCode.Created);
                }
                return ResponseDto<EndpointResponseDto>.Fail("the endpoint was not saved to the database", (int)System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<EndpointResponseDto>> UpdateEndpoint(EndpointRequestDto endpointRequestDto)
        {
            try
            {
                _logger.Information("about to get the instance from the database");
                EndPoint endpointObj = await _unitOfWork.EndpointRepository.GetAsync(x => x.Id == endpointRequestDto.EndpointId);
                if (endpointObj is not null)
                {
                    _logger.Information("the endpointObj was found");
                    _logger.Information("updating the instance gotten from the database with the data gotten from tge DTO");
                    endpointObj.ControllerName = !string.IsNullOrEmpty(endpointRequestDto.ControllerName) ? endpointRequestDto.ControllerName : endpointObj.Method;
                    endpointObj.Method = !string.IsNullOrEmpty(endpointRequestDto.Method) ? endpointRequestDto.Method : endpointObj.Method;
                    endpointObj.Endpoint = !string.IsNullOrEmpty(endpointRequestDto.Endpoint) ? endpointRequestDto.Endpoint : endpointObj.Method;
                    endpointObj.Channel = !string.IsNullOrEmpty(endpointRequestDto.Channel) ? endpointRequestDto.Channel : endpointObj.Method;
                    _logger.Information("have successfully updated the Endpoint instances from the DTO request");
                    _logger.Information("about to add the value to the database instance to be updated");
                    _unitOfWork.EndpointRepository.Update(endpointObj);
                    await _unitOfWork.Save();
                    EndPoint EndpointResp = await _unitOfWork.EndpointRepository.GetAsync(x => x.Id.Equals(endpointObj.Id));
                    _logger.Information("added the value to the database");
                    EndpointResponseDto ReturnDto = _mapper.Map<EndpointResponseDto>(EndpointResp);
                    return ResponseDto<EndpointResponseDto>.Success("successfully saved the new updates added to the database", ReturnDto, (int)System.Net.HttpStatusCode.Accepted);
                }
                return ResponseDto<EndpointResponseDto>.Fail("the endpointObj was not found in the database", (int)System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }
    }
}
