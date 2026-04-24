using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResortAPI.Models;
using ResortAPI.Services;

namespace ResortAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthenticationController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            return await _authService.Register(registerUser, role);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            return await _authService.Login(loginModel);
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok(new Response
            {
                Status = "Success",
                Message = "Logged out successfully"
            });
        }
    }
}
