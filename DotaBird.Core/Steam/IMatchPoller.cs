using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotaBird.Core.Steam
{
    public interface IMatchPoller
    {
        /// <summary>
        /// Continuously poll matches from Steam, returning them one at a time.
        /// </summary>
        IEnumerable<MatchSummary> PollMatches();
    }
}
