using System.Globalization;
using System.Text;
using LocationCheck.External.GoogleMapsPlatform;

namespace LocationCheck.API.Models
{
    
    public class LocationCheckRequest
    {
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public double? Radius { get; set; }
        
        public FilterObject? FilterObject { get; set; }

        internal string GetLocationCoords => new StringBuilder()
            .Append(Latitude.ToString(CultureInfo.InvariantCulture))
            .Append(',')
            .Append(Longitude.ToString(CultureInfo.InvariantCulture))
            .ToString();

        internal double InRadius => Radius.HasValue ? 
            Radius.Value == 0 ? 100 : Radius.Value
            : 100;

    }
}
