using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestDrivenTesingWithRestSharp
{
    public class Place
    {
        [JsonPropertyName("place name")]
        public string PLaceName { get; set; }
        public string State { get; set; }
        public string StateAbbreviation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
