using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpredMedia.UserManagement.API.Controllers
{
    [Route("api/v1/[controller")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet(Name = "Profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            return Ok();
        }
    }
}

