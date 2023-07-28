using Microsoft.AspNetCore.Mvc;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.CommonLibrary;
using System.Net;
using System.Net.Mime;

namespace SpredMedia.Authentication.API.Controllers
{
    [Route("AuthenticationMS/api/v1/ClientAccount")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ClientAccountController : ControllerBase
    {
        private readonly IClientAccountService _clientAccountService;
        public ClientAccountController(IClientAccountService clientAccountService)
        {
            _clientAccountService = clientAccountService;
        }
        /// <summary>
        /// register a client base application for the
        /// TO be granted access to a group of active directory
        /// </summary>
        /// <param name="ClientRequestDto"> a data transfer object that is used to pass object to the endpoint</param>
        /// <returns name =""></returns>
        [HttpPost("register-client")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRequestDto ClientRequestDto)
        {
            var result = await _clientAccountService.RegisterClient(ClientRequestDto);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// used to update the value for the client User
        /// properties 
        /// </summary>
        /// <param name="ClientRequestDto">a data transfer object that is used to pass object to the endpoint</param>
        /// <returns></returns>
        [HttpPost("update-client")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateClient([FromBody] ClientRequestDto ClientRequestDto)
        {
            var result = await _clientAccountService.UpdateClient(ClientRequestDto);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// this method is used to remove a client user application from the database administrator
        /// thus restricting access to our endpoint resource
        /// </summary>
        /// <param name="ClientId">its the identifying value of the endpoint that distinguish the user client app</param>
        /// <returns></returns>
        [HttpDelete("deleteclient/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteClientById([FromRoute] string ClientId)
        {
            var result = await _clientAccountService.DeleteClient(ClientId);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Endpoint that returns client data from the database using username 
        /// </summary>
        /// <param name="ClientUserName">used as parameter to retreive the client user interface</param>
        /// <returns name ="clientSecret"></returns>
        [HttpGet("client")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetClientByUserName()
        {
            var result = await _clientAccountService.GetClientByUser();
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// this endpoint is used to add grants as to how many active 
        /// directory a user client app can be access
        /// </summary>
        /// <param name="ClientId">the value that would be used to distinguish the users client</param>
        /// <returns></returns>
        [HttpGet("Add-clientendpoint/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddClientEndpoint([FromRoute] string ClientId, [FromQuery] string EndpointId)
        {
            var result = await _clientAccountService.AddClientEndpoint(ClientId, EndpointId);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// this is used to get all the client endpoint 
        /// associated to the clientId pass through the route
        /// </summary>
        /// <param name="ClientId">the value that would be used to distinguish the users client</param>
        /// <returns></returns>
        [HttpGet("get-clientendpoint/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllClientEndpoint([FromRoute] string ClientId)
        {
            var result = await _clientAccountService.GetClientEndpoints(ClientId);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        ///  this is used to remove the client application user from accessing an active directory
        /// </summary>
        /// <param name="ClientId">the value that would be used to distinguish the users client</param>
        /// <returns></returns>
        [HttpDelete("delete-clientendpoint/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteClientEndpoint([FromRoute] string ClientId, [FromQuery] string EndpointId)
        {
            var result = await _clientAccountService.DeleteClientEndpoint(ClientId, EndpointId);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        ///  this is used to remove the client application user from accessing an active directory
        /// </summary>
        /// <param name="ClientId">the value that would be used to distinguish the users client</param>
        /// <returns></returns>
        [HttpGet("getallclients")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllClients([FromQuery] int PageSize, [FromQuery] int PageNumber)
        {
            var result = await _clientAccountService.GetAllClient(PageSize, PageNumber);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        ///  this holds the logic that generate the Key and IV for an AES encryption
        /// </summary>
        /// <param name="Values">the value that would be used to distinguish the users client</param>
        /// <returns></returns>
        [HttpPost]
        [Route("getkeyandiv")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetKeyAndIv(KeyAndIv Values)
        {
            var result = await _clientAccountService.GetKeyAndIv(Values);
            return StatusCode(result.StatusCode, result);
        }
    }
}
