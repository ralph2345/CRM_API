using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;
using Crm.Domain.Interfaces;
using Crm.Application.Interfaces;
using Crm.Persistence;
using Crm.Domain.Entities;
using Crm.Application.DTO.Users;
using Crm.Application.DTO.Login;

namespace Crm.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, JwtService jwt, IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwt;
            _emailService = emailService;
        }

        /*public async Task<IEnumerable<UserDto>> SearchUsersByNameAsync(string name)
        {
            var users = await _userRepository.SearchUsersByNameAsync(name);

            return users.Select(MapToDto).ToList();
        }*/

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(string? searchName)
        {
            var users = await _userRepository.GetAllUsersAsync(searchName);

            return users.Select(MapToDto).ToList();
        }

        public async Task<ApiResponseDto> RegisterAsync(RegisterUserDto request)
        {
            if (await _userRepository.UserExistsAsync(request.Email))
                return new ApiResponseDto(false, "User already exists");

            if (request.Password != request.ConfirmPassword)
                return new ApiResponseDto(false, "Passwords do not match");

            if (!IsValidPassword(request.Password))
                return new ApiResponseDto(false, "Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, a number, and a special character.");

            //registering new users
            var user = new Users
            {
                PhotoLink = request.PhotoLink,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Status = "Active",
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            };

            await _userRepository.AddUserAsync(user);
            return new ApiResponseDto(true, "User Registered Successfully!");
        }

        public async Task<LoginResponseDto> LoginAsync(LoginUserDto request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.UserName);

            //checking if the user exists
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password",
                    SessionToken = string.Empty,
                    PhotoLink = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty,
                };
            }

            //checking if the account is active
            if (user.Status != "Active")
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Account is inactive",
                    SessionToken = string.Empty,
                    PhotoLink = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty,
                };
            }

            var token = _jwtService.GenerateToken(user, request.RememberMe);

            //returned data on front end for successful login
            return new LoginResponseDto
            {
                Success = true,
                SessionToken = token,
                PhotoLink = user.PhotoLink,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        public async Task<string> UpdateUserAsync(int userId, UpdateUserDto request)
        {
            //updating users details
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return "User not found";

            user.PhotoLink = request.PhotoLink;
            user.FirstName = request.FirstName;
            user.MiddleName = request.MiddleName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            user.Email = request.Email;

            await _userRepository.UpdateUserAsync(user);
            return "User Updated Successfully!";
        }

        public async Task<string> IsDeactivateUserAsync(bool isDeactivate, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return "User not found";

            await _userRepository.IsDeactivateUserAsync(isDeactivate, userId);

            if (isDeactivate)
            {
                user.Status = "Inactive";
                return "User deactivated successfully";
            }
            else
            {
                user.Status = "Active";
                return "User reactivated successfully";
            }
            
        }

        /*public async Task<string> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return "User not found";

            await _userRepository.DeleteUserAsync(user);
            return "User Deleted Successfully!";
        }*/

        public async Task<ApiResponseDto> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            Console.WriteLine($"Forgot Password Request for Email: {request.Email}");

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                Console.WriteLine("Email is missing in request.");
                return new ApiResponseDto(false, "Email is required.");
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return new ApiResponseDto(false, "The email must match on your saved account");
            }

            byte[] emailBytes = System.Text.Encoding.UTF8.GetBytes(user.Email);
            string encryptedEmail = Convert.ToBase64String(emailBytes);

            user.EmailExpiration = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateUserAsync(user);

            //generate token for request forgotpassword
            //var token = _jwtService.GeneratePasswordResetToken(user);

            //send email with reset password link
            await _emailService.SendPasswordResetEmail(user.Email);
            Console.WriteLine("Email Sent.");

            //return new ApiResponseDto(true, $"{token}");
            return new ApiResponseDto(true, "Email Sent.");
        }


        public async Task<ApiResponseDto> ResetPasswordAsync(ResetPasswordDto request)
        {
            // Trim token to remove any whitespace or trailing characters
            //request.Token = request.Token.TrimEnd('.', ')', '"', ' ');

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                return new ApiResponseDto(false, "User not found");

            // Check if the reset request has expired
            if (user.EmailExpiration == null || user.EmailExpiration < DateTime.UtcNow)
            {
                return new ApiResponseDto(false, "Reset password link has expired.");
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            return new ApiResponseDto(true, "Password reset successfully");
        }

        //comparing entered password into a stored hash password
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }

        //method for password validation
        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        private UserDto MapToDto(Users user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Status = user.Status,
                UserName = user.UserName,
                Email = user.Email
            };
        }

    }
}
