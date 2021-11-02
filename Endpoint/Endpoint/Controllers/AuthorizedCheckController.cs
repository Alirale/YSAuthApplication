using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorizedCheckController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("You Are Successfully Authorized");
        }
    }
}
