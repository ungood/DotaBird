using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            new TwitterHandler().PostOnTwitter(new MatchSummary() { Id = 315893539 }, "Nole_Wovoka", "Sinvalss");
            //new Program().Run();
        }

        private readonly IMatchPoller poller;

        public Program()
        {
            var webClient = new WebClient();
            var api = new DotaWebApi(webClient);
            poller = new MatchPoller(api);
        }

        public void Run() {

            List<MatchSummary> allUniqueMatches = new List<MatchSummary>();

            //var count = CountMatches(allUniqueMatches, TimeSpan.FromMinutes(30));          // 540 matches found -- hmmm? seems low. 
            var count = CountMatches(allUniqueMatches, TimeSpan.FromSeconds(100));           // 502 matches found
            logger.Info("{0} matches counted.", count);
                                   

            Console.WriteLine("Done, press enter.");
            Console.ReadLine();
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