using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crm.Application.Interfaces;
using Crm.Application.DTO.Login;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _authService;
        public AuthController(IUserService authService)
        {
            _authService = authService;
        }

        //Login user
        /*[HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            return Ok(response);
        }*/
    }
}
