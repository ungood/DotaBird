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
            var expected = new DateTime(2013, 08, 21, 05, 45, 45);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUriExtensions()
        {
            DateTime dateTime = new DateTime(2013, 08, 21, 05, 45, 45);
            long startTime = 1377063945;
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=A41B14673A53F4C0A5281A6C47637C9E";


            Uri uri = new Uri(url)
                .AddQuery("player_name", "wovoka")
                .AddQuery("start_at_match_id", startTime)
                .AddQuery("date_min", dateTime)
                .AddQuery("date_max", dateTime);

            string actual = uri.ToString();
            string expected = url + "&player_name=wovoka&start_at_match_id=1377063945&date_min=1377063945&date_max=1377063945";

            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void TestMatchHistory()
        {
            var response = @"
            {
	            result: {
		                    status: 1,
		                    num_results: 25,
		                    total_results: 500,
		                    results_remaining: 475,
		                    matches: [{
				                    match_id: 295198203,
				                    match_seq_num: 268707406,
				                    start_time: 1378049271,
				                    lobby_type: 0,
				                    players: [{
						                    account_id: 37732908,
						                    player_slot: 0,
						                    hero_id: 53
					                    },
                                        {
						                    account_id: 33332908,
						                    player_slot: 1,
						                    hero_id: 46
					                    }]
                                 }]
                            }
                }";

            mockClient.Setup(client => client.Get(It.IsAny<Uri>()))
               .Returns(response);

            MatchHistory history = api.GetMatchHistory(new MatchHistoryRequest());
            Assert.AreEqual(history.NumResults, 25);
            Assert.AreEqual(history.ResultsRemaining, 475);
            Assert.AreEqual(history.Matches[0].Id, 295198203);
            Assert.AreEqual(history.Matches[0].Players[1].HeroId, 46);
        }

        

        [Test]
        public void TestMatchPoller()
        {
            var response = @"
            {
                result: {
                            matches: [{},{},{},{},{},{match_id: 66}]
                        }
            }";

            mockClient.Setup(client => client.Get(It.IsAny<Uri>()))
                .Returns(response);

            var poller = new MatchPoller(api, 100);
            
            MatchSummary match = null;
            var matches = poller.PollMatches().GetEnumerator();
            int count = 0;

            for (int i = 0; i < 6; i++)
            {
                matches.MoveNext();
                match = matches.Current;
                count++;
            }
            
            Assert.AreEqual(match.Id, 66);
            Assert.AreEqual(count, 6);

        }
    }
}
