using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LocationCheck.Security.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                          ILoggerFactory logger,
                                          UrlEncoder encoder,
                                          ISystemClock clock,
                                          IHttpContextAccessor httpContextAccessor)
                                          : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //TODO: move all those strings to constants
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Auth header");
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var authenticationHeader))
            {
                return AuthenticateResult.Fail("Invalid header value");
            }

            if(!string.Equals(authenticationHeader.Scheme, "Basic", StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorised");
            }
            
            //TODO: Here load user data

            var identity = new GenericIdentity("Username");
            var principal = new GenericPrincipal(identity, new[] { "Username" });
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), "Basic");

            return AuthenticateResult.Success(ticket);
        }
    }
}
