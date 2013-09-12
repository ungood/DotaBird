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
        private int count = 0;
        private long lastMatchId;
        private MatchHistory history;
        private const int numResults = 25;
        private MatchHistoryRequest request;
        private bool firstRun = true;

        public MatchPoller(IDotaWebApi api, int throttle)
        {
            this.api = api;
            this.throttle = throttle;

            // initialize history, lastmatchid and request
            history = api.GetMatchHistory();
            lastMatchId = history.Matches[numResults - 1].Id;
            request = new MatchHistoryRequest();
            request.StartAtMatchId = lastMatchId;
            
        }

        public IEnumerable<MatchSummary> PollMatches()
        {
            while (true)
            {
                if (lastMatchId != history.Matches[count].Id || firstRun || count == numResults - 1)
                    yield return history.Matches[count];

                if (count == numResults - 1)
                {
                    request.StartAtMatchId = lastMatchId;

                    count = 0;

                    history = api.GetMatchHistory(request);

                    if (history.ResultsRemaining > 0)               
                        lastMatchId = history.Matches[numResults - 1].Id;
                    else
                        break;       /// Past ~60 seconds of polling, it runs out of results,
                                     /// I thought about calling GetMatchHistory with no request
                                     /// and that works, except, it will cause repeats to happen.
                }

                if (count == numResults - 1)
                    firstRun = false;
                else
                    count++;

                // We don't want to spam Steam's API
                Thread.Sleep(throttle);

            }

            
        }
    }
}
