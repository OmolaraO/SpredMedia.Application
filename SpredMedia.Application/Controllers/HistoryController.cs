using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpredMedia.UserManagement.Core.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpredMedia.UserManagement.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryServices _historyServices;

        public HistoryController(IHistoryServices historyServices)
        {
            _historyServices = historyServices;
        }

        /// <summary>
        /// delete movie download history
        /// </summary>
        /// <param name="downloadId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpDelete("profiles/{profileId}/downloads/{downloadId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDownloadHistory([FromRoute] string downloadId, string profileId)
        {
            var response = await _historyServices.DeleteDownloadHistory(downloadId, profileId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Returns Download History in Pages.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("download-history/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDownloadHistoryAsync([FromRoute] string profileId, int pageNumber)
        {
            var downloadHistory = await _historyServices.GetDownloadHistoryAsync(profileId, pageNumber);
            return StatusCode(downloadHistory.StatusCode, downloadHistory);
        }


        /// <summary>
        /// delete movie view history
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpDelete("profiles/{profileId}/views/{viewId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteViewingHistory([FromRoute] string viewId, string profileId)
        {
            var response = await _historyServices.DeleteViewHistory(viewId, profileId);
            return StatusCode((int)HttpStatusCode.OK, response);
        }


        /// <summary>
        /// Returns Viewing History in Pages.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("view-history/{profileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetViewingHistoryAsync([FromRoute] string profileId, int pageNumber)
        {
            var viewingHistory = await _historyServices.GetViewingHistoryAsync(profileId, pageNumber);
            return StatusCode(viewingHistory.StatusCode, viewingHistory);
        }
    }

}

