using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crm.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Crm.Application.DTO.Login;

namespace CRM_API.Controllers
{
    [Authorize(AuthenticationSchemes = "Basic")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        //Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            var loginResponse = await _userService.LoginAsync(request);

            if (!loginResponse.Success)
            {
                return Unauthorized(new { message = loginResponse.Message });
            }

            return Ok(new
            {
                sessionToken = loginResponse.SessionToken,
                photoLink = loginResponse.PhotoLink,
                firstName = loginResponse.FirstName,
                lastName = loginResponse.LastName,
                email = loginResponse.Email,
                rememberMe = request.RememberMe
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var response = await _userService.ForgotPasswordAsync(request);
            if (!response.Success)
            {
                if (response.Message == "The email must match on your saved account")
                {
                    return NotFound(new { message = response.Message });//404
                }
                return BadRequest(new { message = response.Message });
            }

            return Ok(new { message = response.Message });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var response = await _userService.ResetPasswordAsync(request);
            if (!response.Success)
            {
                return BadRequest(new { message = response.Message });
            }
            return Ok(new { message = response.Message });
        }

    }
}
