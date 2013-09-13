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
    public class UriExtensionsTest
    {
        [Test]
        public void TestUriExtensions()
        {
            DateTime dateTime = new DateTime(2013, 08, 21, 05, 45, 45);
            long matchID = 1377063945;
            string url = "http://example.org/";


            Uri uri = new Uri(url)
                .AddQuery("player_name", "wovoka")
                .AddQuery("start_at_match_id", matchID)
                .AddQuery("date_min", dateTime)
                .AddQuery("date_max", dateTime);

            string actual = uri.ToString();
            string expected = url + "?player_name=wovoka&start_at_match_id=1377063945&date_min=1377063945&date_max=1377063945";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAddQuerywithNullInput()
        {
            string url = "http://example.org/";
            string stringNull = null;
            long? longNull = null;
            DateTime? dateNull = null;

            Uri uri = new Uri(url)
                .AddQuery("player_name", stringNull)
                .AddQuery("start_at_match_id", longNull)
                .AddQuery("date_min", dateNull)
                .AddQuery("date_max", dateNull);

            string actual = uri.ToString();
            string expected = url;

            Assert.AreEqual(expected, actual);

        }
    }
}
