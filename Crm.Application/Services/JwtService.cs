using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Crm.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Crm.Application.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        //Generating token for successful login with user details and session expiration time
        public string GenerateToken(Users user, bool rememberMe = false)
        {
            string jwtKey = Environment.GetEnvironmentVariable("JwtKey") ??
                    _config["JwtKey"];

            var key = Encoding.UTF8.GetBytes(jwtKey);
            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            //generate token depending on rememberMe
            //var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var expirationMinutes = rememberMe
                ? int.Parse(_config["Jwt:ExtendedExpirationInMinutes"]) // 1 week if remember me is checked
                : int.Parse(_config["Jwt:ExpirationInMinutes"]); // 1 hour if remember me is not checked

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                //_config["Jwt:Issuer"],
                //_config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //for password reset token
        /*public string GeneratePasswordResetToken(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15),//15 minutes expiration
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidatePasswordResetToken(string token, Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            // Proceed with validation
            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5) // Add some tolerance
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Ensure token is a valid JWT
                if (validatedToken is not JwtSecurityToken jwtToken || jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    Console.WriteLine("Invalid token format or signature algorithm.");
                    return false;
                }

                if (emailClaim != user.Email)
                {
                    Console.WriteLine($"Token email ({emailClaim}) does not match user email ({user.Email}).");
                    return false;
                }

                Console.WriteLine("Token validation successful!");
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("Token has expired.");
                return false;
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                Console.WriteLine($"Invalid issuer: {ex.Message}");
                return false;
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                Console.WriteLine($"Invalid audience: {ex.Message}");
                return false;
            }
            catch (SecurityTokenValidationException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in token validation: {ex.Message}");
                return false;
            }
        }*/
    }
}
