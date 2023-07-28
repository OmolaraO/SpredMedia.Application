using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpredMedia.Notification.Core.DTOs;
using SpredMedia.Notification.Core.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpredMedia.Notification.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("NotificationMS/api/v1/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Endpoint for sending Single Email to a spred user in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-email-user")]
        public async Task<IActionResult> SendSingleMail(SingleEmailDTO model)
        {
            var result = await _emailService.SendSingleEmail(model);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Endpoint for sending Single Email to a spred user with adress provided by the sender
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-email-at-specified-address")]
        public async Task<IActionResult> SendSingleMailAtAddress(SingleEmailDTO model)
        {
            var result = await _emailService.SendSingleEmailAtAddress(model);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Endpoint for sending Bulk Email to Specific Spred Users
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-specific-bulk-email")]
        public async Task<IActionResult> SendBulkMailToSpecific(BulkEmailDTO model)
        {
            var result = await _emailService.SendBulkEmailToSpecific(model);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Endpoint for sending Bulk Email to All Spred Users in Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-all-bulk-email")]
        public async Task<IActionResult> SendBulkMailToAll(BulkEmailToAllDTO model)
        {
            var result = await _emailService.SendBulkEmailToAll(model);
            return StatusCode(result.StatusCode, result);
        }
    }
}

