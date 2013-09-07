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
        private static MatchHistory historyPrevious = new MatchHistory();
        private MatchHistory history;
        private const int numResults = 25;

        public MatchPoller(IDotaWebApi api, int throttle)
        {
            this.api = api;
            this.throttle = throttle;

            // initialize historyPrevious's ids to 0                        // Initialization not working, getting a null ref exception
            while (historyPrevious.Matches.GetEnumerator().MoveNext())
                historyPrevious.Matches.GetEnumerator().Current.Id = 0;
                
        }

        public IEnumerable<MatchSummary> PollMatches()
        {
           
            while (true)
            {
                bool isUnique = true;
                if (count == 0 || count == numResults)
                {
                    count = 0;
                    history = api.GetMatchHistory(new MatchHistoryRequest());
                }

                while (historyPrevious.Matches.GetEnumerator().MoveNext())
                    if (history.Matches[count].Id == historyPrevious.Matches.GetEnumerator().Current.Id)
                        isUnique = false;

                if (isUnique)
                    yield return history.Matches[count++];

                // We don't want to spam Steam's API
                Thread.Sleep(throttle);
            }
        }
    }
}
