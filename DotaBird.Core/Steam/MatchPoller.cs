using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotaBird.Core.Steam
{
    public class MatchPoller : IMatchPoller
    {
        private readonly IDotaWebApi api;
        private readonly int throttle;
        private static int count = 0;
        private static MatchHistory history;
        private static int numResults = 25;

        public MatchPoller(IDotaWebApi api, int throttle)
        {
            this.api = api;
            this.throttle = throttle;
        }

        public IEnumerable<MatchSummary> PollMatches()
        {
            while (true)
            {
                // IDK if GetMatchHistory always returns 25 results exactly
                // so I implemented it in such way to not break if GetMatchHistory returned less than 25 results.
                bool isMax = false;

                if (count == numResults)
                    isMax = true;

                if (count == 0 || isMax)
                {
                    count = 0;
                    history = api.GetMatchHistory(new MatchHistoryRequest());
                    numResults = history.NumResults;
                }

                yield return history.Matches[count++];

                // We don't want to spam Steam's API
                Thread.Sleep(throttle);
            }
        }
    }
}
