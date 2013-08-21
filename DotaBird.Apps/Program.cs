using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DotaBird.Core.Steam;
using DotaBird.Core.Net;

namespace DotaBird.Apps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var poller = Initialize();

            // With real data, let's up this to say... 30 minutes and see how many matches come through.
            // Try to get some idea of the amount of data we're dealing with.
            var count = CountMatches(poller, TimeSpan.FromSeconds(10));            

            //MatchHistoryRequest request = new MatchHistoryRequest();
            //request.PlayerName = "wovoka";
            //MatchHistory matchHistory = api.GetMatchHistory(request);
            //Console.WriteLine(matchHistory);
            Console.WriteLine("{0} matches counted.", count);
            Console.ReadLine();
        }

        private static IMatchPoller Initialize()
        {
            var webClient = new WebClient();
            var api = new DotaWebApi(webClient);
            return new MatchPoller(api, 100);
        }

        private static long CountMatches(IMatchPoller poller, TimeSpan span)
        {
            var start = DateTime.Now;
            var matches = poller.PollMatches().GetEnumerator();
            var count = 0L;

            while (matches.MoveNext() && ((DateTime.Now - start) < span))
            {
                count++;
                var match = matches.Current;
                Console.WriteLine("Match: {0} @ {1}", match.Id, match.StartTime);
            }

            return count;
        }
    }
}
