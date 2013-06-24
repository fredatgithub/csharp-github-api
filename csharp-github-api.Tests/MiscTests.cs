﻿namespace GitHubAPI.Tests
{
    using NUnit.Framework;
    using RestSharp;
    using RestSharp.Deserializers;

    [TestFixture]
    public class MiscTests
    {
        [Test]
        public void DynamicResponse_WithCustomDeserializer()
        {
            var restRequest = new RestRequest
            {
                Resource = "/users/sgrassie"
            };

            var client = new RestClient
            {
                BaseUrl = "https://api.github.com"
            };
            client.AddHandler("application/json", new DynamicJsonDeserializer());

            dynamic response = client.Execute<dynamic>(restRequest);

            NUnit.Framework.Assert.That(response.Data["login"], Is.StringMatching("sgrassie"));
        }

        [Test]
        public void DynamicReponse_WithExtensionMethod()
        {
            var restRequest = new RestRequest
            {
                Resource = "/users/sgrassie"
            };

            var client = new RestClient
            {
                BaseUrl = "https://api.github.com"
            };

            var response = client.Execute(restRequest);
            var dynamicPart = response.Dynamic();

            NUnit.Framework.Assert.That(dynamicPart.login, Is.StringMatching("sgrassie"));
        }
    }

    public class DynamicJsonDeserializer : IDeserializer
    {
        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return (T)SimpleJson.DeserializeObject(response.Content);
        }
    }
}
