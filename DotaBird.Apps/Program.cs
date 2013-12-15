using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using NLog;

using DotaBird.Core.Steam;
using DotaBird.Core.Net;
using DotaBird.Core.Twitter;

namespace DotaBird.Apps
{
    public class Program : BaseApp
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override string AppName
        {
            get { return "DotaBirdPollingApp"; }
        }

        public static void Main(string[] args)
        {
            new Program().Run();
        }

        private readonly IMatchPoller poller;
        private TwitterHandler twitterHandler;

        public Program()
        {
            var webClient = new WebClient();
            var api = new DotaWebApi(webClient);
            poller = new MatchPoller(api);
            twitterHandler = new TwitterHandler();
        }

        public void Run() {

            List<MatchSummary> allUniqueMatches = new List<MatchSummary>();
            
            while (true)
            {
                var count = CountMatches(allUniqueMatches, TimeSpan.FromMinutes(30));          // Live function
                //var count = CountMatches(allUniqueMatches, TimeSpan.FromSeconds(30));           // Test function
                logger.Info("{0} matches counted.", count);

                // Get the requests from twitter into a text file
                var requests = new ManipulateRequests();
                requests.GetRequestsFromTwitter(twitterHandler);

                // get the requests from text file into memory with a dictionary
                Dictionary<long, string> requestDictionary = new PlayerRequests().GetRequests();

                // search through each player in each match,
                // and check if that playerID matches any player ID from GetRequests()
                // if match, post on twitter.
                foreach (MatchSummary match in allUniqueMatches)
                {
                    foreach (PlayerSummary player in match.Players)
                    {
                        foreach(KeyValuePair<long, string> request in requestDictionary)
                        {
                            if (player.AccountId == request.Key)
                                twitterHandler.PostOnTwitter(match, request.Value, request.Key.ToString());
                        }
                    }
                }
            }
        }

        private long CountMatches(List<MatchSummary> allUniqueMatches, TimeSpan span)
        {
            var start = DateTime.Now;
            var matches = poller.PollMatches().GetEnumerator();
            var count = 0L;

            while (matches.MoveNext() && ((DateTime.Now - start) < span))
            {
                count++;
                var match = matches.Current;

                allUniqueMatches.Add(match);
                logger.Info("Match: {0} @ {1}", match.Id, match.StartTime);
            }

            return count;
        }
    }
}