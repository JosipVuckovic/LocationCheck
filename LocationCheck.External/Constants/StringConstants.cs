using LocationCheck.External.GoogleMapsPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationCheck.External.Constants
{
    internal sealed class StringConstants
    {
        internal const string ApiSettings = "GoogleApiSettings";
        private const string BaseUrl = "BaseUrl";
        internal const string ApiSettingsBaseUrl = $"{ApiSettings}:{BaseUrl}";

        internal const string KeyInRequest = "&key=";
    }
}
