using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestSharpExercises
{
    public class Issues
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("number")]
        public int number { get; set; }
        [JsonPropertyName("title")]
        public string title { get; set; }
        [JsonPropertyName("body")]
        public string body { get; set;}
    }
}
