using CostEventManegement.AuthModule.Models;
using CostEventManegement.AuthModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CostEventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<LoggedUser> Login(LoginDTO loginForm)
        {
            return await _authService.Login(loginForm);
        }

        [HttpPost("register")]
        public async Task<LoggedUser> Register(UserDTO user)
        {
            return await _authService.Register(user);
        }

    }
}
