using Applcation.Services;
using Core.ViewModel;
using Endpoint.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ITokenService _tokenService;

        public UserController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("RegisterInfo")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            var loginResult = await _tokenService.Register(register);
            if (loginResult != null)
            {
                return new ApiResponse().Success(loginResult);
            }
            else
            {
                return new ApiResponse().Failed("Username already exist please use another username");
            }
        }

        [HttpPost("LoginInfo")]
        public async Task<IActionResult> Login(RegisterViewModel login)
        {
            var loginResult = await _tokenService.LoginwithUserPass(login);
            if (loginResult != null)
            {
                return new ApiResponse().Success(loginResult);
            }
            else
            {
                return new ApiResponse().Failed("UserName or Password does not match. please try agin");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> GetTokenByRefreshToken(string RefreshToken)
        {
            var loginResult = await _tokenService.LoginwithRefreshToken(RefreshToken);
            if (loginResult != null)
            {
                return new ApiResponse().Success(loginResult);
            }
            else
            {
                return new ApiResponse().Failed("Invalid RefreshToken");
            }
        }
    }
}
