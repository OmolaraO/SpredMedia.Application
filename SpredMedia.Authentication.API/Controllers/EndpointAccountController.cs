using Microsoft.AspNetCore.Mvc;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Model;
using System.Net.Mime;

namespace SpredMedia.Authentication.API.Controllers
{
    [Route("AuthenticationMS/api/v1/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class EndpointAccountController : ControllerBase
    {
        private readonly IEndpointServices _endpointService;
        public EndpointAccountController(IEndpointServices endpointServices)
        {
            _endpointService = endpointServices;
        }
        /// <summary>
        /// register an endpoint resource for the api
        /// </summary>
        /// <param name="EndpointRequestDto">the request data transfer object</param>
        /// <returns></returns>
        [HttpPost("add-endpoint")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClientEndpoint([FromBody] EndpointRequestDto EndpointRequestDto)
        {
            var result = await _endpointService.RegisterEndpoint(EndpointRequestDto);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// used to update the value for the endpoint resource
        /// </summary>
        /// <param name="EndpointRequestDto">the object model that
        /// contains the value for  the endpoint resource</param>
        /// <returns></returns>
        [HttpPost("update-endpoint")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateClient([FromBody] EndpointRequestDto EndpointRequestDto)
        {
            var result = await _endpointService.UpdateEndpoint(EndpointRequestDto);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// this method is used to remove the endpoint resource
        /// </summary>
        /// <param name="EndpointId">used as a distinguishing factor for seperating endpoint resource</param>
        /// <returns></returns>
        [HttpDelete("delete-endpoint/{endpointId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteClientById([FromRoute] string endpointId )
        {
            var result = await _endpointService.DeleteEndpoint(endpointId);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Get all the endpoints associated withe the clientID
        /// </summary>
        /// <param name="ClientId">the value that would be used to distinguish the Client UserID</param>
        /// <returns></returns>
        [HttpGet("client-endpoints/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetClientEndpoints([FromRoute] string ClientId)
        {
            var result = await _endpointService.GetClients(ClientId);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Get all the endpoints associated withe the clientID
        /// </summary>
        /// <param name="ClientId">the value that would be used to distinguish the Client UserID</param>
        /// <returns></returns>
        [HttpGet("client-endpoints")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllClient([FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            var result = await _endpointService.GetAllEndpoints(pageSize, pageNumber);
            return StatusCode(result.StatusCode, result);
        }
    }
}