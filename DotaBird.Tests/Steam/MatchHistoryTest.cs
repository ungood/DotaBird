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
    public class MatchHistoryTest
    {
        private Mock<IWebClient> mockClient;
        private DotaWebApi api;

        [SetUp]
        public void Setup()
        {
            mockClient = new Mock<IWebClient>();
            api = new DotaWebApi(mockClient.Object);
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
    }
}
