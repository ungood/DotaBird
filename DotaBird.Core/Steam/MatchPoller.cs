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

        public MatchPoller(IDotaWebApi api, int throttle)
        {
            this.api = api;
            this.throttle = throttle;
        }

        public IEnumerable<MatchSummary> PollMatches()
        {
            while (true)
            {
                // Clearly we want to do something a little more useful. here.
                yield return new MatchSummary();

                // We don't want to spam Steam's API
                Thread.Sleep(throttle);
            }
        }
    }
}
