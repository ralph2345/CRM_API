using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
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
        

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
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

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return FailedLoginResponse("Invalid username or password");
            }

            if (user.Status != "Active")
            {
                return FailedLoginResponse("Account is inactive");
            }

            // Generate token
            var token = GenerateRandomToken();
            var encryptedToken = EncryptTokenAES(token);

            // Set token in domain model
            user.SetToken(encryptedToken, request.RememberMe);

            //update user tokens on db
            await _userRepository.UpdateUserAsync(user);

            return new LoginResponseDto
            {
                Success = true,
                SessionToken = user.JWToken,
                PhotoLink = user.PhotoLink,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
        }

        private LoginResponseDto FailedLoginResponse(string message)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = message,
                SessionToken = string.Empty,
                PhotoLink = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
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

            //byte[] emailBytes = System.Text.Encoding.UTF8.GetBytes(user.Email);
            //string encryptedEmail = Convert.ToBase64String(emailBytes);

            user.EmailExpiration = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateUserAsync(user);
            
            //send email with reset password link
            await _emailService.SendPasswordResetEmail(user.Email);
            Console.WriteLine("Email Sent.");

            //return new ApiResponseDto(true, $"{token}");
            return new ApiResponseDto(true, "Email Sent.");
        }


        public async Task<ApiResponseDto> ResetPasswordAsync(ResetPasswordDto request)
        {

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
        private string GenerateRandomToken(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            var builder = new StringBuilder(length);
            foreach (var b in data)
            {
                builder.Append(chars[b % chars.Length]);
            }

            return builder.ToString();
        }

        //encrypting the token using AES
        private string EncryptTokenAES(string plainText, int length = 10)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? "default_key";
            if (string.IsNullOrWhiteSpace(encryptionKey))
            {
                throw new Exception("Encryption key is missing from secrets.");
            }

            using (Aes aes = Aes.Create())
            {
                var keyBytes = new Rfc2898DeriveBytes(encryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65,
                    0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

                aes.Key = keyBytes.GetBytes(32);
                aes.IV = keyBytes.GetBytes(16);

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();

                    byte[] encryptedBytes = ms.ToArray();

                    // Convert to Base64 and filter to only alphanumerics
                    string base64 = Convert.ToBase64String(encryptedBytes);
                    string alphanumeric = new string(base64.Where(c => validChars.Contains(c)).ToArray());

                    // Trim to requested length
                    return alphanumeric.Length > length ? alphanumeric.Substring(0, length) : alphanumeric;
                }
            }
        }

    }
}
