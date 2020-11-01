using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;

namespace TestHackBlazor.Server.Infrastructure.ApiKeys
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ApiKeyHeaderName = "api-key";
        //private readonly ILogger<ApiKeyAuthenticationHandler> _logger;
        private readonly ApplicationDbContext _context;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            //ILogger<ApiKeyAuthenticationHandler> logger,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            ApplicationDbContext context) : base(options, loggerFactory, encoder, clock)
        {
            //_logger = logger;
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                //_logger.LogInformation("API KEY Auth: There is no header.");
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedApiKey))
            {
                //_logger.LogInformation("API KEY Auth: Empty header value.");
                return AuthenticateResult.NoResult();
            }

            if (!Guid.TryParse(providedApiKey, out Guid apiKeyFromHeader))
            {
                return AuthenticateResult.NoResult();
            }


            ApplicationUser existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.ApiKey == apiKeyFromHeader);


            if (existingUser == null)
            {
                //_logger.LogInformation("API KEY Auth: Invalid Key.");
                return AuthenticateResult.Fail("Invalid API Key provided.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingUser.UserName),
                new Claim(AppClaimTypes.ApiKey, existingUser.ApiKey.ToString()),
                new Claim(AppClaimTypes.UserId, existingUser.Id)
            };
            
            //claims.AddRange(existingUser.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            //_logger.LogInformation("API KEY Auth: Success.");

            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            //_logger.LogInformation("API KEY Auth: Handle 401.");

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            //_logger.LogInformation("API KEY Auth: Handle 403.");

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }
    }
}
