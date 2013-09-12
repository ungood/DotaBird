using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            
            //var count = CountMatches(poller, TimeSpan.FromMinutes(30));      // 13,882 matches counted
            var count = CountMatches(poller, TimeSpan.FromSeconds(30));        // 225 matches counted

            Console.WriteLine("{0} matches counted.", count);

            //TestOverlapping();                                                  // for testing purposes

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
            //List<string> myList = new List<string>();                                        // for testing purposes

            while (matches.MoveNext() && ((DateTime.Now - start) < span))
            {
                count++;
                var match = matches.Current;
                Console.WriteLine("Match: {0} @ {1}", match.Id, match.StartTime);
               //myList.Add(match.Id.ToString());                                            // for testing purposes
            }

            //System.IO.File.WriteAllLines(@"C:\Users\Wovoka\Downloads\test.txt", myList);    // for testing purposes

            return count;
        }

         /* For testing purposes:
        private static void TestOverlapping()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Wovoka\Downloads\test.txt");
            int length = lines.Length;
            int numRepeats = 0;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length - 1; j++)
                {
                    if (lines[i] == lines[j] && i != j)
                        numRepeats++;
                }
            }

            Console.WriteLine("Number of repeats counted: {0}", numRepeats);
        } //*/
    }
}
