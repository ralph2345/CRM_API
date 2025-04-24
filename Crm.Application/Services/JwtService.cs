using Microsoft.Extensions.Configuration;

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
        /*public string GenerateToken(Users user, bool rememberMe = false)
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
        }*/

        //for password reset token

    }
}
