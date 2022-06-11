using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Net;
using System.Threading.Tasks;

namespace TestDrivenTesingWithRestSharp
{
    public class ZipopotamusApiTesting
    {
       

        [TestCase("BG", "1000", "Sofija")]
        [TestCase("BG", "8600", "Jambol")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("DE", "01067", "Dresden")]
        [TestCase("GB", "B1", "Birmingham")]
        public async Task TestZaipopotamus(string countryCode, string zipCode, string expectedPlace)
        {
            var client = new RestClient("https://api.zippopotam.us");
            var request = new RestRequest(countryCode + "/" + zipCode);

            var response = await client.ExecuteAsync(request, Method.Get);
            var location = new SystemTextJsonSerializer().Deserialize<Location>(response);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(countryCode, Is.EqualTo(location.CountryAbbreviation));
            Assert.That(zipCode, Is.EqualTo(location.postCode));
            StringAssert.Contains(expectedPlace, location.Places[0].PLaceName);
        }
       
    }
}