using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using Crm.Persistence;

namespace CRM_API
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly CrmDbContext _context;
        private IOptionsMonitor<BasicAuthenticationOptions> _options;

        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            CrmDbContext context) : base(options, logger, encoder, clock)
        {
            _context = context;
            _options = options;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Check if authorization header exists
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Logger.LogWarning("No Authorization header present");
                return AuthenticateResult.NoResult();
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            Logger.LogInformation($"Auth header received: {authHeader}");

            try
            {
                // Find the token in the database
                var token = await _context.ApiTokens
                    .FirstOrDefaultAsync(t => t.ApiToken == authHeader && t.Status == 1);

                if (token == null)
                {
                    return AuthenticateResult.Fail("Invalid token.");
                }

                // Create claims from the token data
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, token.Name),
                    new Claim(ClaimTypes.Role, token.Role)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }
        }
    }
}