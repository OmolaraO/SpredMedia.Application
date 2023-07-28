using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Core.Utility;
using SpredMedia.Authentication.Model;
using SpredMedia.CommonLibrary;
using EasyEncryption;
using System.Security.Cryptography;
using System.Net;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using SpredMedia.Authentication.Model.model;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace SpredMedia.Authentication.Core.Services
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientAccountService(IServiceProvider service)
        {
            _mapper = service.GetRequiredService<IMapper>();
            _logger = service.GetRequiredService<ILogger>();   
            _unitOfWork =  service.GetRequiredService<IUnitOfWork>();   
            _httpContextAccessor = service.GetRequiredService<IHttpContextAccessor>();
        }
        public async Task<ResponseDto<bool>> AddClientEndpoint(string ClientId, string EndpointId)
        {
            try
            {
                _logger.Information("fetching the insatance of the client from the database");
                var checkClient = await _unitOfWork.ClientRepository.GetAllAsync().Include("Endpoints").AsNoTracking().FirstAsync(x => x.Id == ClientId);
                _logger.Information("checking the value to know if the client exist");
                if (checkClient is not null)
                {
                    _logger.Information("checking if the endpoint is a valid endpoint on the administrator");
                    if(await _unitOfWork.EndpointRepository.GetAsync(x=> x.Id.Equals(EndpointId)) is not null)
                    {
                        if(checkClient.Endpoints.Any(x => x.Id == EndpointId))
                        {
                            _logger.Information("checking to know if the endpoint already exist as one of the clients endpoints");
                            return ResponseDto<bool>.Fail("the client is already profiled for this endpoint", (int)HttpStatusCode.NotAcceptable);
                        }
                        _logger.Information("Client Exist in the database Record");
                        await _unitOfWork.ClientEndpointRepository.InsertAsync(new ClientEndpoint { ClientID = ClientId,EndPointId = EndpointId });
                        await _unitOfWork.Save();
                        return ResponseDto<bool>.Success("the client has been giving access to the endpoint", true, (int)HttpStatusCode.Created);
                    }
                    return ResponseDto<bool>.Fail("the endpoint is not valid", (int)HttpStatusCode.NotAcceptable);
                }
                return ResponseDto<bool>.Fail("the client was not found in the database, So we can add an endpoint to Null", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);   
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<bool>> DeleteClient(string ClientId)
        {
            try
            {
                _logger.Information("about to remove the client from the database");
                var client = await _unitOfWork.ClientRepository.GetAsync(x => x.Id.Equals(ClientId));   
                if (client is not null)
                {
                    _logger.Information("the instance exist in the database");
                    _logger.Information("proceeding to delete instance from from the database");
                    await _unitOfWork.ClientRepository.DeleteAsync(ClientId);
                    await _unitOfWork.Save();
                    return ResponseDto<bool>.Success("successfully saved the new updates added to the database", true, (int)HttpStatusCode.Accepted);
                }
                return ResponseDto<bool>.Fail("the client doesnt exit in the database", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<bool>> DeleteClientEndpoint(string ClientId, string EndpointId)
        {
            try
            {
                _logger.Information("about to remove the client from the database");
                var client = _unitOfWork.ClientEndpointRepository
                                        .GetAllAsync().Where(x => x.ClientID.Equals(ClientId)
                                        && x.EndPointId.Equals(EndpointId)).AsEnumerable();
                if (client is not null)
                {
                    _logger.Information("the instance exist in the database");
                    _logger.Information("proceeding to delete instance from from the database");
                     _unitOfWork.ClientEndpointRepository.DeleteRangeAsync( client);
                    await _unitOfWork.Save();
                    return ResponseDto<bool>.Success("successfully saved the new updates added to the database", true, (int)HttpStatusCode.Accepted);
                }
                return ResponseDto<bool>.Fail("the client doesnt exit in the database", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<List<EndpointResponseDto>>> GetClientEndpoints(string ClientId)
        {
            try
            {
                _logger.Information("get the client entity using the entity framework");
                Client clientEntity = await _unitOfWork.ClientRepository.GetAllAsync().Include("Endpoints").AsNoTracking().FirstAsync(x => x.Id == ClientId);
                if (clientEntity is not null)
                {
                    _logger.Information("Getting the list of the model enpoint associated to the client Instances");
                    var listOfEndpoint = clientEntity.Endpoints.ToList();
                    if(listOfEndpoint.Any())
                    {
                        List<EndpointResponseDto> listOfEndPointResponse = listOfEndpoint.Select(x =>_mapper.Map<EndpointResponseDto>(x)).ToList();
                        _logger.Information($"{listOfEndpoint.Count} endpoints count are listed for the user Client application");
                        if(listOfEndPointResponse.Count>0)
                        {
                            return ResponseDto<List<EndpointResponseDto>>.Success("the client endpoint are", listOfEndPointResponse, (int)HttpStatusCode.Accepted);
                        }
                    }
                    return ResponseDto<List<EndpointResponseDto>>.Fail("they are not endpoint associated to the service client", (int)HttpStatusCode.OK);
                }
                return ResponseDto<List<EndpointResponseDto>>.Fail("the client does not Exist in the database", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<ClientResponseDto>> GetClientByUser()
        {
            try
            {
                _logger.Information("about to get the instance fro the database");
                var password = _httpContextAccessor.HttpContext.Request.Headers["password"].ToString();
                var ClientUserName = _httpContextAccessor.HttpContext.Request.Headers["username"].ToString();
                _logger.Information("getting the username " + ClientUserName + " from the httpcontext");
                _logger.Information("gets the client instance fror the user");
                var clientUser = _unitOfWork.ClientRepository.GetAsync(x => x.ClientUsername == ClientUserName).Result;
                if (clientUser is not null)
                {
                    _logger.Information("the value exist in the database of the user");
                    _logger.Information("getting the Key and IV to be used in decrypting the client secret");
                    var KeyToDecryptSecret = Encryption.GenerateSHA256(password).Substring(0, 32);
                    var IVToDecryptSecret = Encryption.GenerateSHA256(ClientUserName).Substring(0, 16);
                    _logger.Information("decrpty the client Secret");
                    clientUser.ClientSecret = Encryption.DecryptData(clientUser.ClientSecret, KeyToDecryptSecret, IVToDecryptSecret);
                    _logger.Information("used the pagination to limit the traffic of data coming from the database");
                    _logger.Information("passing into the dto, the value from the client database");
                    ClientResponseDto resp = _mapper.Map<ClientResponseDto>(clientUser);
                    return ResponseDto<ClientResponseDto>.Success("Here is the Client details please secure this details", resp, (int)HttpStatusCode.Accepted);
                }
                return ResponseDto<ClientResponseDto>.Fail("the client does not Exist in the database", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<ClientResponseDto>> RegisterClient(ClientRequestDto ClientRequestDto)
        {
            try
            {
                _logger.Information($"checkig if the client with the email already exist " + ClientRequestDto.CompanyEmail);
                if( await _unitOfWork.ClientRepository.GetAsync(x => x.CompanyEmail == ClientRequestDto.CompanyEmail) != null)
                {
                    return ResponseDto<ClientResponseDto>.Fail("the client already exist", (int)HttpStatusCode.NotAcceptable);
                }
                _logger.Information("the client is about to be created withe following model " + JsonConvert.SerializeObject(ClientRequestDto));
                // map the client dto to the client Request model
                _logger.Information("mapping Dto to the object model"); 
                Client ClientObject = _mapper.Map<Client>(ClientRequestDto);
                // generate the algorithm for developing the username and also hashing the password
                _logger.Information("Generating the clientId and Client Secret from the client Company Username");
                string ClientId = ClientObject.ClientUsername + RandomAlgorithm.Generate4Digit().ToString();
                string ClientSecret = RandomAlgorithm.RandomizeString(ClientObject.ClientPassword);
                _logger.Information("Successfully developed the secret and the ClientId");
                // develop the algorithm for creating the userSecret and then encrypting the secrete
                _logger.Information("generating the IV and Key for decrypting the body parameter");
                string DecryptSecretIV = Encryption.GenerateSHA256(ClientObject.ClientUsername).Substring(0, 16);
                string DecryptSecretKey = Encryption.GenerateSHA256(ClientObject.ClientPassword).Substring(0, 32);
                ClientObject.Id = ClientId;
                ClientObject.ClientSecret = Encryption.EncryptData(ClientSecret, DecryptSecretKey, DecryptSecretIV);
                ClientObject.ClientPassword = SHA.ComputeSHA256Hash(ClientObject.ClientPassword);
                // develop the algorithm
                // add the value of the client instance to the sql server by calling the entity frame work
                await _unitOfWork.ClientRepository.InsertAsync(ClientObject);
                await _unitOfWork.Save();
                _logger.Information("the instance have been added to the database");
                // get the instance from the session just to make sure that the instance has saved
                Client ClientResp = await _unitOfWork.ClientRepository.GetAsync(x => x.Id.Equals(ClientObject.Id));
                if (ClientResp != null)
                {
                    _logger.Information("successfully saved to the database ");
                    // mapping the client details to the 
                    ClientResponseDto Resp = _mapper.Map<ClientResponseDto>(ClientResp);
                    Resp.ClientSecret = ClientSecret;
                    Resp.ClientId = ClientId;
                    return ResponseDto<ClientResponseDto>.Success("the client has been created successful", Resp, (int)HttpStatusCode.Created);
                }
                return ResponseDto<ClientResponseDto>.Fail("the client was not saved to the database", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<ClientResponseDto>> UpdateClient(ClientRequestDto ClientRequestDto)
        {
            try
            {   
                _logger.Information("about to get the instance fro the database");
                var password = _httpContextAccessor.HttpContext.Request.Headers["password"];
                var username = _httpContextAccessor.HttpContext.Request.Headers["username"];
                _logger.Information("getting the username " + username +" from the httpcontext");
                Client client = await  _unitOfWork.ClientRepository.GetAsync(x => 
                                        SHA.ComputeSHA256Hash(password).Equals(x.ClientPassword) 
                                        && x.ClientUsername.Equals(username));
                if(client is not null)
                {
                    _logger.Information("the client was found");
                    _logger.Information("updating the instance gotten from the database with the data gotten from tge DTO");
                    client.ClientPassword = SHA.ComputeSHA256Hash(ClientRequestDto.ClientPassword);
                    client.ClientUsername = ClientRequestDto.ClientUsername;
                    client.CompanyName = ClientRequestDto.CompanyName;
                    client.CompanyEmail = ClientRequestDto.CompanyEmail;
                    client.CompanyPhone = ClientRequestDto.CompanyPhone;
                    _logger.Information("have successfully updated the cleint instances from the DTO request");
                    _logger.Information("about to add the value to the database instance to be updated");
                    _unitOfWork.ClientRepository.Update(client);
                    await _unitOfWork.Save();
                    _logger.Information("added the value to the database");
                    ClientResponseDto ReturnDto = _mapper.Map<ClientResponseDto>(client);
                    return ResponseDto<ClientResponseDto>.Success("successfully saved the new updates added to the database, the client secret is returned as encrypted", ReturnDto, (int)HttpStatusCode.Accepted);
                }
                return ResponseDto<ClientResponseDto>.Fail("the client was not found in the database", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<PaginationResult<IEnumerable<ClientResponseDto>>>> GetAllClient(int pageSize, int pageNumber)
        {
            try
            {
                _logger.Information("getting all the client from the clientDB");
                var listOfClient = _unitOfWork.ClientRepository.GetAllAsync();
                _logger.Information("used the pagination to limit the traffic of data coming from the database");
                var ClientValue = await listOfClient.PaginationAsync<Client, ClientResponseDto>(pageSize,pageNumber,_mapper);
                return ResponseDto<PaginationResult<IEnumerable<ClientResponseDto>>>.Success("successfully saved the client added to the database", ClientValue, (int)HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _unitOfWork.Dispose();
                throw;
            }
        }

        public async Task<ResponseDto<KeyAndIv>> GetKeyAndIv(KeyAndIv keyAndIv)
        {
            try
            {
                _logger.Information("getting all the client from the clientDB  for the user "+ keyAndIv.ValueForIV);
                var Client = await _unitOfWork.ClientRepository.GetAsync(x => x.ClientPassword.Equals
                                   (SHA.ComputeSHA256Hash(keyAndIv.ValueForKey)) 
                                   && x.ClientUsername.Equals(keyAndIv.ValueForIV));
                _logger.Information("getting the Key and IV to be used in decrypting the client secret");
                var KeyToDecryptSecret = Encryption.GenerateSHA256(keyAndIv.ValueForKey).Substring(0, 32);
                var IVToDecryptSecret = Encryption.GenerateSHA256(keyAndIv.ValueForIV).Substring(0, 16);
                _logger.Information("decrpty the client Secret");
                Client.ClientSecret = Encryption.DecryptData(Client.ClientSecret, KeyToDecryptSecret, IVToDecryptSecret);
                _logger.Information("used the pagination to limit the traffic of data coming from the database");
                keyAndIv.ValueForIV = Encryption.GenerateSHA256(Client.ClientSecret).Substring(0, 16);
                keyAndIv.ValueForKey = Encryption.GenerateSHA256(Client.Id).Substring(0, 32);
                return ResponseDto<KeyAndIv>.Success("successfully retrive and converted the value to key and IV", keyAndIv, (int)HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                _unitOfWork.Dispose();
                throw;
            }
        }
    }
}
