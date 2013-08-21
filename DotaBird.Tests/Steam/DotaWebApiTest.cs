using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using DotaBird.Core.Net;
using DotaBird.Core.Steam;

namespace DotaBird.Tests.Steam
{
    [TestFixture]
    public class DotaWebApiTest
    {
        private Mock<IWebClient> mockClient;
        private DotaWebApi api;

        [SetUp]
        public void Setup()
        {
            // Setup is run before every test case.

            // We are going to test the DotaWebApi by injecting a 
            // mock implementation of IWebClient.  A mock is a class that implements
            // an interface in a dummy or trivial way, allowing us to test code that
            // depends on that interface.
            // 
            // Moq is a mocking framework that create mock objects at runtime.
            
            // First we create mock instance of IWebClient.
            mockClient = new Mock<IWebClient>();

            // Then we inject that into the class we want to test.
            api = new DotaWebApi(mockClient.Object);
        }

        [Test]
        public void ExampleTestWithMoq()
        {
            // Here we are saying, for this test only, if our mock object's Get
            // method is called with any Uri as the parameter, return the string "{}"
            mockClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns("{}");

            // Now we can test GetMatchHistory without ever calling Steam's service.
            // We're testing just the logic that is in DotaWebApi, in isolation, which is
            // much easier than trying to test the logic of everything together.
            MatchHistory actual = api.GetMatchHistory(new MatchHistoryRequest());
            Assert.IsNull(actual);

            // See: https://code.google.com/p/moq/wiki/QuickStart for more examples of using Moq.
        }

        [Test]
        public void TestSteamDateConverter()
        {
            var response = @"
            {
                result: {
                    matches: [{
                        start_time: 1377063945
                    }]
                }
            }";

            mockClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(response);

            MatchHistory history = api.GetMatchHistory(new MatchHistoryRequest());
            var actual = history.Matches[0].StartTime;
            var expected = new DateTime(2013, 08, 20, 22, 45, 45);
            Assert.AreEqual(expected, actual);
        }
    }
}
