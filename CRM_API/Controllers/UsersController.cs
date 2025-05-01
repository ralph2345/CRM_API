using Crm.Application.DTO.Users;
using Crm.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Basic", Policy = "ApiKey")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers(string? search)
        {
            var users = await _userService.GetAllUsersAsync(search);
            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }

        //Registering user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Will return validation error details if the DTO is not valid.
            }
            var response = await _userService.RegisterAsync(request);
            if (!response.Success)
            {
                if (response.Message.Contains("User already exist", StringComparison.OrdinalIgnoreCase))
                {
                    return Conflict(response);
                }
                return BadRequest(response);
            }

            return Created("", response);
        }


        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto request)
        {
            if (request == null)
            {
                return BadRequest(new {message = "Invalid request data" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Will return validation error details if the DTO is not valid.
            }

            var result = await _userService.UpdateUserAsync(userId, request);

            if (result == "User not found")
            {
                return NotFound(new {message = "User not found" });
            }

            return Ok(new {message = result });
        }

        [HttpPut("v2/is-deactivate")]
        public async Task<IActionResult> DeactivateUser([FromQuery]bool isDeactivate,[FromQuery]List<int> userId)
        {
            var result = await _userService.IsDeactivateUserAsync(isDeactivate, userId);
            if (result == "User not found")
                return NotFound(new { message = result });

            return Ok(new { message = result });
        }

    }
}
