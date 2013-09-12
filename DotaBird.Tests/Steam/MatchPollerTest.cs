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
    public class MatchPollerTest
    {
        private Mock<IDotaWebApi> mockWebAPI;

        [SetUp]
        public void Setup()
        {
            mockWebAPI = new Mock<IDotaWebApi>();
        }


        [Test]
        public void TestMatchPoller()
        {
            MatchHistory history = new MatchHistory();
            history.Matches[5].Id = 66;

            mockWebAPI.Setup(webAPI => webAPI.GetMatchHistory(It.IsAny<MatchHistoryRequest>()))
                .Returns(history);

            var poller = new MatchPoller(mockWebAPI.Object, 100);   // I still don't know how to fix this
 
            MatchSummary match = null;
            var matches = poller.PollMatches().GetEnumerator();
            int count = 0;

            for (int i = 0; i < 6; i++)
            {
                matches.MoveNext();
                match = matches.Current;
                count++;
            }

            Assert.AreEqual(66, match.Id);
            Assert.AreEqual(6, count);
        }
    }
}
