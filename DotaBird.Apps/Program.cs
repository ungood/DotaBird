using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NLog;

using DotaBird.Core.Steam;
using DotaBird.Core.Net;

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

        public static bool testMode = true;

        private readonly IMatchPoller poller;

        public Program()
        {
            var webClient = new WebClient();
            var api = new DotaWebApi(webClient);
            poller = new MatchPoller(api);
        }

        public void Run() {
                                                  
            
            //// With real data, let's up this to say... 30 minutes and see how many matches come through.
            //// Try to get some idea of the amount of data we're dealing with.
            
            //var count = CountMatches(poller, TimeSpan.FromMinutes(30), myList);          // 540 matches found -- hmmm? seems low. 
            var count = CountMatches(TimeSpan.FromMinutes(30));        // 502 matches found
            logger.Info("{0} matches counted.", count);

            //if (testMode)
            //    TestOverlapping(myList);                                                  

            Console.WriteLine("Done, press enter.");
            Console.ReadLine();
        }

        private long CountMatches(TimeSpan span)
        {
            var start = DateTime.Now;
            var matches = poller.PollMatches().GetEnumerator();
            var count = 0L;

            while (matches.MoveNext() && ((DateTime.Now - start) < span))
            {
                count++;
                var match = matches.Current;
                
                logger.Info("Match: {0} @ {1}", match.Id, match.StartTime);
            }

            return count;
        }

        private static void TestOverlapping(List<string> myList)
        {
            string[] lines = myList.ToArray();
            int length = lines.Length;
            int numRepeats = 0;

            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length - 1; j++)
                {
                    if (lines[i] == lines[j] && i != j)
                    {
                        numRepeats++;
                        break;
                    }
                    
                }
            }

            Console.WriteLine("Number of repeats counted: {0}", numRepeats);
        } 
    }
}
