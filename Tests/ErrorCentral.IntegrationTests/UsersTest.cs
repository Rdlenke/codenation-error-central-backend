using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ErrorCentral.IntegrationTests
{
    public class UsersTest : ApiTestFixture
    {
        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private const string baseUrl = "/api/v1/users";
        public HttpClient Client { get; }
        public UsersTest(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

    }
}
