using Crm.Application.DTO.Users;
using Crm.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _authService;

        public AccountsController(IUserService authService)
        {
            _authService = authService;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        //Registering user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(new { message = result });
        }


        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto request)
        {
            if (request == null)
            {
                return BadRequest(new { code = 400, message = "Invalid request data" });
            }

            var result = await _authService.UpdateUserAsync(userId, request);

            if (result == "User not found")
            {
                return NotFound(new { code = 404, message = "User not found" });
            }

            return Ok(new { code = 200, message = result });
        }


        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _authService.DeleteUserAsync(userId);
            return Ok(new { message = result });
        }
    }
}
