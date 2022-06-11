using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestSharpExercises
{
    public class Tests
    {
        private const string url = "https://api.github.com";
        private RestClient client;
        private RestRequest request;
        private const string username = "Ralitsa-Djurkova";
        private const string token = "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";

        private string singleIssueURL = "/repos/Ralitsa-Djurkova/postman/issues/{id}";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(url);

            this.client.Authenticator = new HttpBasicAuthenticator(username, token);
            
        }

        [Test]
        public void Test_GitHub_APIRequest()
        {
            this.request = new RestRequest("/repos/dimosoftuni/postman/issues");
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void Test_All_Issues()
        {
            this.request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issues");
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issues = JsonSerializer.Deserialize<List<Issues>>(response.Content);

            Assert.That(issues.Count > 1);
            foreach (var issue in issues)
            {

                Assert.That(issues.Count, Is.GreaterThan(0));
                Assert.Greater(issue.id, 0);
                Assert.Greater(issue.number, 0);
                Assert.IsNotEmpty(issue.title);
            }
        }
        private async Task<Issues> CreateIssue(string title, string body)
        {
            this.client.Authenticator = new HttpBasicAuthenticator("Ralitsa-Djurkova", "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");
            var request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issues");
            request.AddBody(new { body, title });
            var response = await this.client.ExecuteAsync(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issues>(response.Content);
            return issue;
        }
        [Test]
        public async Task Test_Create_GitHubIssueAsync()
        {
            string title = "new issue from Restsharp";
            string body = "Some body here";
            var issue = await CreateIssue(title, body);

            Assert.Greater(issue.id, 0);
            Assert.Greater(issue.number, 0);
            Assert.IsNotEmpty(issue.title);
        }
        [Test]
        public async Task Test_Create_GitHubIssueAsync_UnauthenticatedPost()
        {
            this.request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issues");
            this.client.Authenticator = new HttpBasicAuthenticator("Ralitsa-Djurkova", "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1");
            string title = "New issue from RestSharp";
            string body = "Some body here";
            var response = await this.client.ExecuteAsync(request, Method.Post);
            var issue = await CreateIssue(title, body);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task Test_Create_InvalidPost_with_Missintitle()
        {
            this.request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issues");
            
            string title = " ";
            string body = "Some body here";
            var response = await this.client.ExecuteAsync(request, Method.Post);
            var issue = await CreateIssue(title, body);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
        }
        private async Task<Issues> PatchIssue(string title, string body)
        {
            var request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issie/76");
            request.AddBody(new { body, title });
            var response = await this.client.ExecuteAsync(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issues>(response.Content);
            return issue;
        }
        [Test]
        public async Task Test_Delete_With_Missing_Authenticator()
        {
            var request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issues");
            request.AddUrlSegment("id", 7);

            //Act
            var response = await this.client.ExecuteAsync(request, Method.Delete);  
            var issue = JsonSerializer.Deserialize<Issues>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task Test_Path_With_Authenticator()
        {
            this.client.Authenticator = new HttpBasicAuthenticator(username, token);

            this.request = new RestRequest("/repos/Ralitsa-Djurkova/postman/issues/7");
            request.AddUrlSegment("id", 7);

            var newTitle = "Change title from restSharp";

            //Act
            request.AddJsonBody(new { title = newTitle, body = "Body" });

            var response = await this.client.ExecuteAsync(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issues>(response.Content);

            //Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issue.title, Is.EqualTo((string)newTitle));
        }
        [Test]
        public async Task Test_With_Missing_Authenticator()
        {
            this.request = new RestRequest("repos/Ralitsa_Djurkova/postman/issues/5");
            request.AddUrlSegment("id", 5);

            var newTitle = "Changed title from RestSharp";

            //Act
            request.AddBody(new { newTitle });
            var response = await this.client.ExecuteAsync(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issues>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
           
        }
    }
}