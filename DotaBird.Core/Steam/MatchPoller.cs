using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;

namespace DotaBird.Core.Steam
{
    public class MatchPoller : IMatchPoller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IDotaWebApi api;

        public int MinThrottle { get; set; }
        public int MaxThrottle { get; set; }

        public int currentThrottle;
        
        public MatchPoller(IDotaWebApi api)
        {
            this.api = api;
            MinThrottle = 100;
            MaxThrottle = 30 * 1000;
            currentThrottle = MinThrottle;
        }

        List<long> ids = new List<long>();

        public IEnumerable<MatchSummary> PollMatches()
        {           
            const int ONE_MILLION = 1000000;
            
            while(true) 
            {
                bool foundDuplicate = false;
                int uniqueCount = 0;
                foreach (var summary in GetUniqueMatches(ref foundDuplicate))
                {
                    uniqueCount++;
                    yield return summary;
                }

                if (!foundDuplicate)
                {
                    logger.Warn("Failed to find a duplicate.  We may be missing some matches!");
                }

                if (ids.Count == ONE_MILLION)
                {
                    // 1 long = 8 bytes; 1 million longs = 8 MB.  I don't want to exceed that much ram used,
                    // so when that limit is reached, I clear the oldest/bottom half of the list.  
                    // This will probably never happen.
                    ids.RemoveRange(0, ONE_MILLION / 2);
                }

                // If there's not even one match found, double the throttle time and wait some more,
                // otherwise half it.
                if (uniqueCount == 0)
                {
                    logger.Info("No new matches found.");
                    currentThrottle = Math.Min(currentThrottle * 2, MaxThrottle);
                } else {
                    currentThrottle = Math.Max(currentThrottle / 2, MinThrottle);
                }            

                logger.Info("Sleep for {0} ms.", currentThrottle);
                Thread.Sleep(currentThrottle);
            }
        }

        public IEnumerable<MatchSummary> GetUniqueMatches(ref bool foundDuplicate) {
            List<MatchSummary> uniqueMatches = new List<MatchSummary>();

            foreach(var summary in GetNextBatch()) {
                // Remember what we learned about data structures, and the run time.
                // Is Contains an efficient operation to be doing on a list?
                // I think there are better ways you can come up with for checking duplicates.
                // Hint: the matches are always returned in order, most recent first.
                if (ids.Contains(summary.Id))
                {
                    foundDuplicate = true;
                    break;
                }

                ids.Add(summary.Id);
                uniqueMatches.Add(summary);
            }

            return uniqueMatches;
        }

        public IEnumerable<MatchSummary> GetNextBatch()
        {
            foreach (MatchHistory history in GetPages())
            {
                foreach (MatchSummary match in history.Matches)
                {
                    yield return match;
                }

                Thread.Sleep(MinThrottle);
            }
        }

        public IEnumerable<MatchHistory> GetPages() 
        {
            MatchHistory history = api.GetMatchHistory();
            yield return history;

            while(history.ResultsRemaining > 0) 
            {
                MatchHistoryRequest request = new MatchHistoryRequest()
                {
                    StartAtMatchId = history.GetLastMatchId() - 1
                };
                history = api.GetMatchHistory(request);
                yield return history;
            }
        }
    }
}
