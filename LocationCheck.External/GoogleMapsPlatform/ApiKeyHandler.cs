using LocationCheck.External.Constants;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace LocationCheck.External.GoogleMapsPlatform
{
    internal class ApiKeyHandler : DelegatingHandler
    {
        private readonly GoogleApiSettings _settings;
        private readonly string _apiKey;

        public ApiKeyHandler(IOptions<GoogleApiSettings> options)
        {
            _settings = options.Value;
            _apiKey = StringConstants.KeyInRequest+options.Value.ApiKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancelToken)
        {           

            var uriBuilder = new UriBuilder(request.RequestUri);

            if (string.IsNullOrEmpty(uriBuilder.Query))
            {
                uriBuilder.Query = _apiKey;
            }            
            else
            {
                uriBuilder.Query = uriBuilder.Query+_apiKey;
            }
            
            request.RequestUri = uriBuilder.Uri;


            return await base.SendAsync(request, cancelToken);
        }
    }
}
