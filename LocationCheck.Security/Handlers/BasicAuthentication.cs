using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text.Encodings.Web;
using LocationCheck.Data;
using LocationCheck.Security.Constants;
using Microsoft.EntityFrameworkCore;

namespace LocationCheck.Security.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LocationCheckDb _locationCheckDb;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                          ILoggerFactory logger,
                                          UrlEncoder encoder,
                                          ISystemClock clock,
                                          IHttpContextAccessor httpContextAccessor,
                                          LocationCheckDb locationCheckDb)
                                          : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
            _locationCheckDb = locationCheckDb;
        }

        //TODO: move all those strings to constants
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(StringConstants.Authorization))
            {
                return AuthenticateResult.Fail(StringConstants.MissingAuthHeader);
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[StringConstants.Authorization], out var authenticationHeader))
            {
                return AuthenticateResult.Fail(StringConstants.InvalidHeader);
            }

            if(!string.Equals(authenticationHeader.Scheme, StringConstants.Basic, StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateResult.Fail(StringConstants.Unauthorised);
            }
            
            if (string.IsNullOrEmpty(authenticationHeader.Parameter))
            {
                return AuthenticateResult.Fail(StringConstants.Unauthorised);
            }
            
            if (!Guid.TryParse(authenticationHeader.Parameter, out var apiKey))
            {
                return AuthenticateResult.Fail(StringConstants.Unauthorised);
            }

            var user = await _locationCheckDb.ApiUsers.Where(_ => _.ApiKey == apiKey).FirstOrDefaultAsync();

            if (user is null)
            {
                return AuthenticateResult.Fail(StringConstants.Unauthorised);
            }
            
            var identity = new GenericIdentity(user.Id.ToString());
            var principal = new GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), StringConstants.Basic);

            return AuthenticateResult.Success(ticket);
        }
    }
}
