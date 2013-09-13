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
        public static bool testMode = true;

        public static void Main(string[] args)
        {
            List<string> myList = new List<string>();                                        
            var poller = Initialize();

            // With real data, let's up this to say... 30 minutes and see how many matches come through.
            // Try to get some idea of the amount of data we're dealing with.
            
            var count = CountMatches(poller, TimeSpan.FromMinutes(30), myList);          // 540 matches found -- hmmm? seems low. 
            //var count = CountMatches(poller, TimeSpan.FromSeconds(30), myList);        // 502 matches found

            Console.WriteLine("{0} matches counted.", count);


            if (testMode)
                TestOverlapping(myList);                                                  


            Console.ReadLine();
        }

        private static IMatchPoller Initialize()
        {
            var webClient = new WebClient();
            var api = new DotaWebApi(webClient);
            return new MatchPoller(api, 100);
        }

        private static long CountMatches(IMatchPoller poller, TimeSpan span, List<string> myList)
        {
            var start = DateTime.Now;
            var matches = poller.PollMatches().GetEnumerator();
            var count = 0L;

            while (matches.MoveNext() && ((DateTime.Now - start) < span))
            {
                count++;
                var match = matches.Current;
                Console.WriteLine("Match: {0} @ {1}", match.Id, match.StartTime);

                if (testMode)
                    myList.Add(match.Id.ToString());                                            

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
